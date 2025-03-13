function AutoCompletePrivacyScreenGetColourID(source, eventArgs) {
    var id = eventArgs.get_value();
    var name = eventArgs.get_text();

    // Remove the colour tag from the name
    name = name.replace(std, '');
    name = name.replace(prem, '');
    name = name.replace(prest, '');
    
    document.getElementById(ctrlPSColourID).value = id;
    document.getElementById(ctrlPSColourName).value = name;
    document.getElementById(ctrlPSColour).title = name;
}

function AutoCompletePrivacyScreenStart(sender, e) {
    sender._element.className = sender._element.className + ' auto-complete-loading';
}

function AutoCompletePrivacyScreenEnd(sender, e) {
    var re = new RegExp(' auto-complete-loading', 'g');
    sender._element.className = sender._element.className.replace(re, '');
}

function AutoCompletePrivacyScreenEnsureValues() {
    document.getElementById(ctrlPSColour).value = document.getElementById(ctrlPSColourName).value;
}