function exampleButtonExecute() {
    engine.trigger('OnExampleButtonClick');    
}

function potatoButtonExecute() {
    engine.trigger('OnPotatoButtonClick');
}

function windowCloseExecute(element) {
    var windowId = element.getAttribute('data-window-id');
    engine.trigger('OnWindowCloseClick', windowId);
}

// If it already exists dont load or execute it
if (typeof enableDrag !== 'function') {
    function enableDrag() {
        // Select all the titlebars within the custom-panel class
        const titleBars = document.querySelectorAll('.custom-panel .custom-panel-titlebar');

        titleBars.forEach(titleBar => {
            let offsetX = 0;
            let offsetY = 0;
            let dragElement = null;

            titleBar.addEventListener('mousedown', function (e) {
                // Prevent default dragging behaviour
                e.preventDefault();

                // Get the current mouse position
                offsetX = e.clientX - this.closest('.custom-panel').offsetLeft;
                offsetY = e.clientY - this.closest('.custom-panel').offsetTop;

                // Reference the closest .custom-panel which needs to move
                dragElement = this.closest('.custom-panel');

                // Set the z-index of window to one more than the highest z-index
                var highestZIndex = getHighestWindowZIndex();
                dragElement.style.zIndex = highestZIndex + 1;

                // Listen for mousemove events on the whole document
                document.addEventListener('mousemove', moveElement, true);
            }, true);

            document.addEventListener('mouseup', function () {
                // Stop moving when mouse button is released
                document.removeEventListener('mousemove', moveElement, true);
                dragElement = null;
            }, true);

            function moveElement(e) {
                // Don't do anything if we aren't dragging
                if (dragElement === null) return;

                // Calculate the new position
                let newLeft = e.clientX - offsetX;
                let newTop = e.clientY - offsetY;

                // Set the element's new position
                dragElement.style.position = 'absolute';
                dragElement.style.top = `${newTop}px`;
                dragElement.style.left = `${newLeft}px`;
            }
        });
    }

    enableDrag();
}

if (typeof hideCustomWindow !== 'function') {
    function hideCustomWindow(elementID) {
        var windowElement = document.getElementById(elementID);
        if (windowElement)
            windowElement.classList.add('hidden');
    }
}

if (typeof showCustomWindow !== 'function') {
    function showCustomWindow(elementID) {
        var windowElement = document.getElementById(elementID);
        if (windowElement)
            windowElement.classList.remove('hidden');
    }
}

if (typeof setupCustomWindows !== 'function') {
    function setupCustomWindows() {
        var closeButtons = document.getElementsByClassName("custom-close-button");

        if (closeButtons && closeButtons.length > 0) {
            for (var i = 0; i < closeButtons.length; i++) {
                var closeButton = closeButtons[i];
                closeButton.onclick = function () {
                    windowCloseExecute(this);
                }
            }
        }
    }
    setupCustomWindows();
}

if (typeof getHighestWindowZIndex !== 'function') {
    // Function to get the highest z-index
    function getHighestWindowZIndex() {
        var panels = document.querySelectorAll('.custom-panel');
        var maxZ = 0;
        panels.forEach(function (panel) {
            var zIndex = parseInt(window.getComputedStyle(panel).zIndex, 10);
            if (zIndex > maxZ) maxZ = zIndex;
        });
        return maxZ;
    }
}

// Call this function once the DOM is fully loaded or at the end of the body
function LoadExampleMod() {
    var button = document.getElementById("example-button");
    button.onclick = exampleButtonExecute;

    button = document.getElementById("potato-button");
    button.onclick = potatoButtonExecute;
}

LoadExampleMod();