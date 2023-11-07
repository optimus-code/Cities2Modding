### UI CSS Variables Documentation

This is a list of CSS  variables available within the Cities Skylines II UI system.

---

###Defaults:

| Variable Name                       | Description                                       |
|-------------------------------------|---------------------------------------------------|
| `--gap1`                            | Presumably a small gap or spacing value (1px).    |
| `--gap2`                            | A small gap or spacing value (1px).               |
| `--gap3`                            | A medium gap or spacing value (2px).              |
| `--gap4`                            | A medium gap or spacing value (2px).              |
| `--gap5`                            | A larger gap or spacing value (3px).              |
| `--gap6`                            | A larger gap or spacing value (3px).              |
| `--gap7`                            | An even larger gap or spacing value (4px).        |
| `--gap8`                            | An even larger gap or spacing value (4px).        |
| `--stroke1`                         | A thin border or stroke value (1px).              |
| `--stroke2`                         | A thin border or stroke value (1px).              |
| `--stroke3`                         | A medium border or stroke value (2px).            |
| `--stroke4`                         | A medium border or stroke value (2px).            |
| `--normalTextColor`                 | Default text color (white).                       |
| `--normalTextColorDim`              | Dimmed text color (#d9d9d9).                      |
| `--normalTextColorDimmer`           | Even more dimmed text color (#b3b3b3).            |
| `--normalTextColorDimmest`          | The dimmest text color before becoming dark (#9a9a9a). |
| `--normalTextColorDark`             | Dark text color with some transparency (rgba(0,0,0,0.8)). |
| `--normalTextColorDarkDim`          | Dark dimmed text color (rgba(0,0,0,0.6)).         |
| `--normalTextColorDarkDimmer`       | Even darker dimmed text color (rgba(0,0,0,0.4)).  |
| `--normalTextColorDarkDimmest`      | The dimmest dark text color (rgba(0,0,0,0.2)).    |
| `--normalTextColorHighlight`        | Highlight color for text, relies on another variable (`--accentColorNormal`). |
| `--normalTextColorHighlightDim`     | Dimmed highlight color for text, relies on another variable (`--accentColorLight`). |
| `--normalTextColorHighlightDimmer`  | Even more dimmed highlight color for text, relies on another variable (`--accentColorLighter`). |
| `--normalTextColorHighlightDimmest` | The dimmest highlight color for text, relies on another variable (`--accentColorLightest`). |
| `--normalTextColorDisabled`         | Color for disabled text (#bfbfbf).                |
| `--normalTextColorLocked`           | Color for locked text with transparency (#b4b4b4cc). |
| `--textColor`                       | General text color, aliased to `--normalTextColor`. |
| `--textColorDim`                    | Dimmed general text color, aliased to `--normalTextColorDim`. |
| `--textColorDimmer`                 | Even more dimmed general text color, aliased to `--normalTextColorDimmer`. |
| `--textColorDimmest`                | The dimmest general text color, aliased to `--normalTextColorDimmest`. |
| `--textColorDark`                   | Dark general text color, aliased to `--normalTextColorDark`. |
| `--textColorDarkDim`                | Dark dimmed general text color, aliased to `--normalTextColorDarkDim`. |
| `--textColorDarkDimmer`             | Even darker dimmed general text color, aliased to `--normalTextColorDarkDimmer`. |
| `--textColorDarkDimmest`            | The dimmest dark general text color, aliased to `--normalTextColorDarkDimmest`. |
| `--textColorHighlight`              | General highlight text color, aliased to `--normalTextColorHighlight`. |
| `--textColorHighlightDim`           | Dimmed general highlight text color, aliased to `--normalTextColorHighlightDim`. |
| `--textColorHighlightDimmer`        | Even more dimmed general highlight text color, aliased to `--normalTextColorHighlightDimmer`. |
| `--textColorHighlightDimmest`       | The dimmest general highlight text color, aliased to `--normalTextColorHighlightDimmest`. |
| `--textColorDisabled`               | General disabled text color, aliased to `--normalTextColorDisabled`. |
| `--textColorLocked`                 | General locked text color, aliased to `--normalTextColorLocked`. |
| `--focusedTextColor`                | Text color for focused elements (rgba(0,0,0,0.7)). |
| `--focusedTextColorDim`             | Dimmed text color for focused elements (rgba(0,0,0,0.6)). |
| `--focusedTextColorDimmer`          | Even more dimmed text color for focused elements (rgba(0,0,0,0.5)). |
| `--focusedTextColorDimmest`         | The dimmest text color for focused elements (rgba(0,0,0,0.4)). |
| `--focusedTextColorDark`            | Dark text color for focused elements (black).     |
| `--focusedTextColorDarkDim`         | Dark dimmed text color for focused elements (rgba(0,0,0,0.9)). |
| `--focusedTextColorDarkDimmer`      | Even darker dimmed text color for focused elements (rgba(0,0,0,0.8)). |
| `--focusedTextColorDarkDimmest`     | The dimmest dark text color for focused elements (rgba(0,0,0,0.7)). |
| `--focusedTextColorHighlight`       | Highlight text color for focused elements (white). |
| `--focusedTextColorHighlightDim`    | Dimmed highlight text color for focused elements (rgba(255,255,255,0.9)). |
| `--focusedTextColorHighlightDimmer` | Even more dimmed highlight text color for focused elements (rgba(255,255,255,0.8)). |
| `--focusedTextColorHighlightDimmest`| The dimmest highlight text color for focused elements (rgba(255,255,255,0.7)). |
| `--focusedTextColorLocked`          | Text color for locked focused elements (rgb(100,100,100)). |
| `--selectedTextColor`               | Text color for selected elements (rgba(0,0,0,0.6)). |
| `--selectedTextColorDim`            | Dimmed text color for selected elements (rgba(0,0,0,0.5)). |
| `--selectedTextColorDimmer`         | Even more dimmed text color for selected elements (rgba(0,0,0,0.4)). |
| `--selectedTextColorDimmest`        | The dimmest text color for selected elements (rgba(0,0,0,0.3)). |
| `--selectedTextColorDark`           | Dark text color for selected elements (rgba(0,0,0,0.9)). |
| `--selectedTextColorDarkDim`        | Dark dimmed text color for selected elements (rgba(0,0,0,0.8)). |
| `--selectedTextColorDarkDimmer`     | Even darker dimmed text color for selected elements (rgba(0,0,0,0.7)). |
| `--selectedTextColorDarkDimmest`    | The dimmest dark text color for selected elements (rgba(0,0,0,0.6)). |
| `--selectedTextColorHighlight`      | Highlight text color for selected elements (rgba(255,255,255,0.9)). |
| `--selectedTextColorHighlightDim`   | Dimmed highlight text color for selected elements (rgba(255,255,255,0.8)). |
| `--selectedTextColorHighlightDimmer`| Even more dimmed highlight text color for selected elements (rgba(255,255,255,0.7)). |
| `--selectedTextColorHighlightDimmest`| The dimmest highlight text color for selected elements (rgba(255,255,255,0.6)). |
| `--selectedTextColorLocked`         | Text color for locked selected elements (rgb(150,150,150)). |
| `--fontScale`                       | Scale factor for font sizes.                       |
| `--fontSizeXS`                      | Extra small font size.                             |
| `--fontSizeS`                       | Small font size.                                   |
| `--fontSizeM`                       | Medium font size.                                  |
| `--fontSizeL`                       | Large font size.                                   |
| `--fontSizeXL`                      | Extra large font size.                             |
| `--panelOpacityNormal`              | Normal opacity for panels.                         |
| `--panelOpacityDark`                | Dark opacity for panels.                           |
| `--panelColorNormal`                | Normal color for panels.                           |
| `--panelColorDark`                  | Dark color for panels.                             |
| `--panelColorDark-hover`            | Dark panel color on hover.                         |
| `--panelColorDark-active`           | Dark panel color when active.                      |
| `--pausePanelColorDark`              | Color for panels during a pause state.            |
| `--scrollbarColor`                   | Color for scrollbars.                             |
| `--panelRadius`                      | Border radius for panel elements.                 |
| `--panelRadiusInner`                 | Inner border radius for nested panel elements.    |
| `--panelRadiusInnerSIP`              | Inner border radius for panel elements, possibly in a specific state or context. |
| `--floatingToggleSize`               | Size for floating toggle elements.                |
| `--toolbarToggleSize`                | Size for toolbar toggle elements.                 |
| `--floatingToggleBorderRadius`       | Border radius for floating toggle elements.       |
| `--dialogButtonYesColor`             | Color for 'Yes' buttons in dialogs.               |
| `--dialogButtonYesColor-hover`       | Hover color for 'Yes' buttons in dialogs.         |
| `--dialogButtonYesColor-active`      | Active color for 'Yes' buttons in dialogs.        |
| `--dialogButtonYesColor-focused`    | Focus color for 'Yes' buttons in dialogs.         |
| `--dialogButtonNoColor`              | Color for 'No' buttons in dialogs.                |
| `--dialogButtonNoColor-hover`        | Hover color for 'No' buttons in dialogs.          |
| `--dialogButtonNoColor-active`       | Active color for 'No' buttons in dialogs.         |
| `--dialogButtonNoColor-focused`      | Focus color for 'No' buttons in dialogs.          |
| `--normalColor`                      | Normal state color, typically for background usage. |
| `--hoverColor`                       | Hover state color.                                |
| `--hoverColorNormal`                 | Normal hover state color, aliases `--hoverColor`. |
| `--hoverColorBright`                 | Brighter variant of hover state color.            |
| `--hoverColorDark`                   | Darker variant of hover state color.              |
| `--activeColor`                      | Active state color.                               |
| `--activeColorNormal`                | Normal active state color, aliases `--activeColor`. |
| `--activeColorBright`                | Brighter variant of active state color.            |
| `--activeColorDark`                  | Darker variant of active state color.              |
| `--disabledColor`                    | Color for disabled elements.                      |
| `--focusedColor`                     | Color for focused elements.                       |
| `--focusedColorDark`                 | Dark color for focused elements.                  |
| `--selectedColor`                    | Color for selected elements.                      |
| `--selectedColorDark`                | Dark color for selected elements.                 |
| `--selectedColor-hover`              | Hover color for selected elements.                |
| `--selectedColor-active`             | Active color for selected elements.               |
| `--buttonSelectAnimation`            | Animation for button selection.                   |
| `--editorFocusedColor`               | Color for focused elements in an editor context.  |
| `--editorFocusedColorDark`           | Dark color for focused elements in an editor context. |
| `--progressColor`                    | Color for progress elements.                      |
| `--progressTextColor`                | Text color for elements showing progress.         |
| `--centerPanelWidth`                 | Width for the center panel.                       |
| `--centerPanelHeight`                | Height for the center panel.                      |
| `--centerPanelSidebarWidth`          | Width for the sidebar in the center panel.        |
| `--rightPanelWidth`                  | Width for the right panel.                        |
| `--leftPanelWidth`                   | Width for the left panel.                         |
| `--sectionHeaderColor`               | Color for section headers.                        |
| `--sectionHeaderColorLight`          | Light color variant for section headers.          |
| `--sectionHeaderLockedColor`         | Color for locked section headers.                 |
| `--sectionBackgroundColor`           | Background color for sections.                    |
| `--sectionBackgroundColorLight`      | Light background color variant for sections.      |
| `--sectionBackgroundLockedColor`     | Background color for locked sections.             |
| `--sectionBorderColor`               | Border color for sections.                        |
| `--dividerColor`                     | Color for dividers.                               |
| `--dividerColorInverted`             | Inverted color for dividers.                      |
| `--chartBorderColor`                 | Border color for charts.                          |
| `--lockedColor`                      | Color indicating a locked state.                  |
| `--unlockedColor`                    | Color indicating an unlocked state.               |
| `--requirementsTitleColor`           | Color for titles of requirements.                 |
| `--requirementFieldColor`            | Background color for requirement fields.          |
| `--requirementFieldProgressColor`    | Progress color for requirement fields.            |
| `--toolbarFieldColor`                | Background color for toolbar fields.              |
| `--toolbarFieldColor-hover`          | Hover color for toolbar fields.                   |
| `--toolbarFieldColor-active`         | Active color for toolbar fields.                  |
| `--toolbarFieldBorderColor`          | Border color for toolbar fields.                  |
| `--toolbarFieldBorderWidth`          | Border width for toolbar fields.                  |
| `--toolbarFieldRadius`               | Border radius for toolbar fields.                 |
| `--toolbarFieldInnerRadius`          | Inner border radius for toolbar fields.           |
| `--accentColorDarker`                | Darker variant of the accent color.               |
| `--accentColorDarker-hover`          | Hover state for the darker variant of the accent color. |
| `--accentColorDarker-pressed`        | Active state for the darker variant of the accent color. |
| `--accentColorDark`                  | Dark variant of the accent color.                 |
| `--accentColorDark-hover`            | Hover state for the dark variant of the accent color. |
| `--accentColorDark-pressed`          | Active state for the dark variant of the accent color. |
| `--accentColorDark-focused`          | Focus state for the dark variant of the accent color. |
| `--policyIconSize`                   | Size for policy icons.                            |
| `--accentColorNormal`                | Normal state of the accent color.                 |
| `--accentColorNormal-hover`          | Hover state for the normal variant of the accent color. |
| `--accentColorNormal-pressed`        | Active state for the normal variant of the accent color. |
| `--accentColorLight`                 | Light variant of the accent color.                |
| `--accentColorLighter`               | Lighter variant of the accent color.              |
| `--accentColorLightest`              | Lightest variant of the accent color.             |
| `--positiveColor`                    | Color that typically indicates success or a positive outcome. |
| `--warningColor`                     | Color that typically indicates a warning or something to be cautious about. |
| `--negativeColor`                    | Color that typically indicates an error or a negative outcome. |
| `--iconColor`                        | Default color for icons.                          |
| `--iconSize`                         | Default size for icons.                           |
| `--iconWidth`                        | Width for icons, often used in conjunction with `--iconSize`. |
| `--iconHeight`                       | Height for icons, often used in conjunction with `--iconSize`. |
| `--sliderRangeColor`                 | Color for the range of sliders.                   |
| `--sliderRangeColorFocused`          | Color for the range of sliders when focused.      |
| `--sliderRangeColorDisabled`         | Color for the range of sliders when disabled.     |
| `--sliderThumbColor`                 | Color for the thumb of sliders.                   |
| `--sliderThumbColorDisabled`         | Color for the thumb of sliders when disabled.     |
| `--sliderThumbColorFocused`          | Color for the thumb of sliders when focused.      |
| `--menuHoverColor`                   | Hover color for menus.                            |
| `--menuHoverColorBright`             | Bright hover color for menus.                     |
| `--menuActiveColor`                  | Active color for menu items.                      |
| `--menuActiveColorInverted`          | Inverted active color for menu items.             |
| `--menuActiveColorBright`            | Bright active color for menu items.               |
| `--menuFocusedColor`                 | Focused color for menu items.                     |
| `--menuSelectedColor`                | Selected color for menu items.                    |
| `--menuSelectedColorSubtle`          | Subtle selected color for menu items.             |
| `--menuDisabledColor`                | Disabled color for menu items.                    |
| `--menuBorderColor1`                 | Primary border color for menus.                   |
| `--menuBorderColor1Inverted`         | Inverted primary border color for menus.          |
| `--menuBorderColor2`                 | Secondary border color for menus.                 |
| `--menuBorderColor2Inverted`         | Inverted secondary border color for menus.        |
| `--menuPanel1`                       | Background color for primary menu panels.         |
| `--menuPanel2`                       | Background color for secondary menu panels.      |
| `--menuBlur`                         | Filter for blurring menu backgrounds.  
| `--menuTitleNormal`            | Color for menu titles in their normal state.        |
| `--menuText1Normal`            | Primary color for menu text in normal state.        |
| `--menuText1Inverted`          | Inverted primary color for menu text.               |
| `--menuText1Disabled`          | Disabled state color for primary menu text.         |
| `--menuText2Normal`            | Secondary color for menu text in normal state.      |
| `--menuText2Inverted`          | Inverted secondary color for menu text.             |
| `--menuControl1`               | Primary color for menu controls.                    |
| `--menuControl1-hover`         | Hover color for primary menu controls.              |
| `--menuControl1-pressed`       | Pressed color for primary menu controls.            |
| `--menuControl2`               | Secondary color for menu controls.                  |
| `--menuControlBorder`          | Border color for menu controls.                     |
| `--assetMenuRows`              | Number of rows in the asset menu.                   |
| `--upgradeMenuRows`            | Number of rows in the upgrade menu.                 |
| `--assetMenuColumns`           | Number of columns in the asset menu.                |
| `--assetMenuItemSize`          | Size for items within the asset menu.               |
| `--assetMenuItemGap`           | Gap size between items in the asset menu.           |
| `--assetMenuImageSize`         | Size for images within the asset menu items.        |
| `--linkColor`                  | Color for link text.                                |
| `--paragraphGap`               | Vertical gap size between paragraphs.               |
| `--panelBlur`                  | Blur effect applied to panel backgrounds.           |
| `--backdropBlur`               | Blur effect applied to backdrop elements.           |
| `--childOpacityTransitionDuration` | Duration for opacity transitions on child elements. |
| `--childOpacityTransitionDelay` | Delay for opacity transitions on child elements.    |
| `--childOpacityTransitionTimingFunction` | Timing function for opacity transitions on child elements. |
| `--selectAnimation`            | Animation name for select interactions.             |
| `--selectDuration`             | Duration for select animations.                     |
| `--tooltipColor`               | Color for tooltips.                                 |
| `--tooltipFilter`              | Filter effect applied to tooltips.                  |
| `--keyboardHintColor`          | Color for keyboard hint indicators.   

---

###".style--bright-blue":

| Variable Name                       | Description                                              |
|-------------------------------------|----------------------------------------------------------|
| `--panelColorNormal`                | Normal color for panels with a bright blue tint.         |
| `--panelColorDark`                  | Dark color for panels with a blue tint.                  |
| `--panelRadius`                     | Border radius for panel elements with a large radius.    |
| `--panelRadiusInner`                | Inner border radius for nested panel elements.           |
| `--accentColorNormal`               | Normal accent color with a bright blue shade.            |
| `--accentColorNormal-hover`         | Hover state for the normal bright blue accent color.     |
| `--accentColorNormal-pressed`       | Pressed state for the normal bright blue accent color.   |
| `--accentColorDark`                 | Dark variant of the bright blue accent color.            |
| `--accentColorDark-hover`           | Hover state for the dark bright blue accent color.       |
| `--accentColorDark-pressed`         | Pressed state for the dark bright blue accent color.     |
| `--accentColorDark-focused`         | Focused state for the dark bright blue accent color.     |
| `--accentColorLight`                | Light variant of the bright blue accent color.           |
| `--accentColorLighter`              | Lighter variant of the bright blue accent color.         |
| `--textColorDimmer`                 | Dimmed text color against bright blue backgrounds.       |
| `--menuPanel1`                      | Background color for primary menu panels (bright blue).  |
| `--menuPanel2`                      | Background color for secondary menu panels (light blue). |
| `--menuDisabledColor`               | Disabled color for menu items against blue backgrounds.  |
| `--menuTitleNormal`                 | Color for menu titles (white for contrast with blue).    |
| `--menuText1Normal`                 | Primary color for menu text against blue.                |
| `--menuText1Inverted`               | Inverted primary color for menu text against blue.       |
| `--menuText1Disabled`               | Disabled state color for primary menu text.              |
| `--menuText2Normal`                 | Secondary color for menu text against blue.              |
| `--menuText2Inverted`               | Inverted secondary color for menu text against blue.     |
| `--menuControl1`                    | Primary color for menu controls against blue.            |
| `--menuControl1-hover`              | Hover color for primary menu controls against blue.      |
| `--menuControl1-pressed`            | Pressed color for primary menu controls against blue.    |
| `--menuControl2`                    | Secondary color for menu controls against blue.          |
| `--menuControlBorder`               | Border color for menu controls against blue.             |

---

###".style--dark-grey-orange"

| Variable Name                       | Description                                              |
|-------------------------------------|----------------------------------------------------------|
| `--panelColorNormal`                | Normal color for panels with a dark grey tint.           |
| `--panelColorDark`                  | Dark color for panels, leaning towards black.            |
| `--panelRadius`                     | Border radius for panel elements with a moderate radius. |
| `--panelRadiusInner`                | Inner border radius for nested panel elements.           |
| `--accentColorNormal`               | Normal accent color with an orange shade.                |
| `--accentColorNormal-hover`         | Hover state for the normal orange accent color.          |
| `--accentColorNormal-pressed`       | Pressed state for the normal orange accent color.        |
| `--accentColorDark`                 | Dark variant of the orange accent color.                 |
| `--accentColorDark-hover`           | Hover state for the dark orange accent color.            |
| `--accentColorDark-pressed`         | Pressed state for the dark orange accent color.          |
| `--accentColorDark-focused`         | Focused state for the dark orange accent color.          |
| `--accentColorLight`                | Light variant of the orange accent color.                |
| `--accentColorLighter`              | Lighter variant of the orange accent color.              |
| `--menuPanel1`                      | Background color for primary menu panels (dark grey).    |
| `--menuPanel2`                      | Background color for secondary menu panels (darker grey).|
| `--menuTitleNormal`                 | Color for menu titles (white for contrast with dark).    |
| `--menuText1Normal`                 | Primary color for menu text against dark grey.           |
| `--menuText1Inverted`               | Inverted primary color for menu text against dark grey.  |
| `--menuText1Disabled`               | Disabled state color for primary menu text.              |
| `--menuText2Normal`                 | Secondary color for menu text against dark grey.         |
| `--menuText2Inverted`               | Inverted secondary color for menu text against dark grey.|
| `--menuControl1`                    | Primary color for menu controls against dark grey.       |
| `--menuControl1-hover`              | Hover color for primary menu controls against dark grey. |
| `--menuControl1-pressed`            | Pressed color for primary menu controls against dark grey.|
| `--menuControl2`                    | Secondary color for menu controls against dark grey.     |
| `--menuControlBorder`               | Border color for menu controls against dark grey.        |