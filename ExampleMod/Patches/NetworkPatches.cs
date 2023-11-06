using Game.Rendering;
using Game.UI.Localization;
using Game.UI.Tooltip;
using Game.UI.Widgets;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace ExampleMod.Patches
{
    // The user must have their unit type set to Metric currently,
    // improvements to handle the various settings and do the relevant
    // conversion would be a good improvement to make.

    /// <summary>
    /// Change length display to Units like Cities Skylines 1
    /// </summary>
    [HarmonyPatch( typeof( NetCourseTooltipSystem ), "OnUpdate" )]
    class NetCourseTooltipSystem_OnUpdatePatch
    {
        static void Postfix( NetCourseTooltipSystem __instance )
        {
            var m_Length = Traverse.Create( __instance ).Field( "m_Length" ).GetValue<FloatTooltip>( );

            if ( m_Length != null )
            {
                m_Length.value /= 8f; // Convert to Cities 1 units
                m_Length.unit = "floatTwoFractions"; // Change to a generic unit type to stop showing m/ft
                m_Length.label = LocalizedString.Value( "U" ); // Adjust the label to say 'U'
            }
        }
    }

    /// <summary>
    /// Ensure the guide line system supports unit display
    /// </summary>
    /// <remarks>
    /// (Also changes IntTooltip to FloatTooltip to ensure values aren't truncated.)
    /// </remarks>
    [HarmonyPatch( typeof( GuideLineTooltipSystem ), "OnUpdate" )]
    class GuideLineTooltipSystem_OnUpdatePatch
    {
        static bool Prefix( GuideLineTooltipSystem __instance )
        {
            var m_TooltipUISystem = Traverse.Create( __instance ).Field( "m_TooltipUISystem" ).GetValue<TooltipUISystem>( );
            var m_Groups = Traverse.Create( __instance ).Field( "m_Groups" ).GetValue<List<TooltipGroup>>( );
            var m_GuideLinesSystem = Traverse.Create( __instance ).Field( "m_GuideLinesSystem" ).GetValue<GuideLinesSystem>( );

            var tooltips = m_GuideLinesSystem.GetTooltips( out var dependencies );
            dependencies.Complete( );

            for ( var index = 0; index < tooltips.Length; ++index )
            {
                var tooltipInfo = tooltips[index];

                if ( m_Groups.Count <= index )
                {
                    var tooltipGroup = new TooltipGroup( );
                    tooltipGroup.path = ( PathSegment ) string.Format( "guideLineTooltip{0}", index );
                    tooltipGroup.horizontalAlignment = TooltipGroup.Alignment.Center;
                    tooltipGroup.verticalAlignment = TooltipGroup.Alignment.Center;

                    // Used to be an IntTooltip, we change this a float to allow for accuracy instead
                    // of truncation.
                    tooltipGroup.children.Add( new FloatTooltip( ) );
                    m_Groups.Add( tooltipGroup );
                }

                var group = m_Groups[index];

                var tooltipPos = WorldToTooltipPos( ( Vector3 ) tooltipInfo.m_Position );

                if ( !group.position.Equals( tooltipPos ) )
                {
                    group.position = tooltipPos;
                    group.SetChildrenChanged( );
                }

                var child = group.children[0] as FloatTooltip;
                var type = tooltipInfo.m_Type;

                switch ( type )
                {
                    // No modifications to angle we just need to ensure its value is passed
                    case GuideLinesSystem.TooltipType.Angle:
                        child.icon = "Media/Glyphs/Angle.svg";
                        child.value = tooltipInfo.m_IntValue;
                        child.unit = "angle";
                        break;

                    // Modify the length field to show units instead
                    case GuideLinesSystem.TooltipType.Length:
                        child.icon = "Media/Glyphs/Length.svg";
                        child.value = tooltipInfo.m_IntValue / 8f; // Convert to Cities 1 units
                        child.unit = "floatTwoFractions"; // Change to a generic unit type to stop showing m/ft
                        child.label = LocalizedString.Value( "U" ); // Adjust the label to say 'U'
                        break;
                }
                AddGroup( m_TooltipUISystem, group );
            }
            return false; // Skip original function
        }

        private static void AddGroup( TooltipUISystem tooltipUISystem, TooltipGroup group )
        {
            if ( group.path != PathSegment.Empty && tooltipUISystem.groups.Any( ( g => g.path == group.path ) ) )
                Debug.LogError( string.Format( "Trying to add tooltip group with duplicate path '{0}'", group.path ) );
            else
                tooltipUISystem.groups.Add( group );
        }

        private static float2 WorldToTooltipPos( Vector3 worldPos )
        {
            var xy = ( ( float3 ) Camera.main.WorldToScreenPoint( worldPos ) ).xy;
            xy.y = Screen.height - xy.y;
            return xy;
        }
    }
}
