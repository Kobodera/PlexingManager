function WebForm_FireDefaultButton(event, target) 
{
    alert("Testing");
    var element = event.target || event.srcElement;
    if (!__defaultFired && event.keyCode == 13 && !(element && (element.tagName.toLowerCase() == "textarea"))) {
        var defaultButton;
        if (__nonMSDOMBrowser) {
            defaultButton = document.getElementById(target);
        }
        else {
            defaultButton = document.all[target];
        }

        if (defaultButton && typeof (defaultButton.click) != "undefined") {
            __defaultFired = true;
            defaultButton.click();
            event.cancelBubble = true;
            if (event.stopPropagation) event.stopPropagation();
            return false;
        }
    }

    return true;
} 