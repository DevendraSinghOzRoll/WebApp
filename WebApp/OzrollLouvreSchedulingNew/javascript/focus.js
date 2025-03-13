
// Add this class to radio button control to indicate focus in UI.
// Register function in Page_Load - ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "styleFocusElements", "styleFocusElements();", True)

function styleFocusElements() {
    // Main function to style all.
    styleRadioButtons();
    styleCheckboxes();
}

function styleRadioButtons() {
    $(".radio").addClass("radio-no-focus");

    $(".radio").focusin(function () {
        $(this).addClass("radio-focus");
        $(this).removeClass("radio-no-focus");
    });
    $(".radio").focusout(function () {
        $(this).removeClass("radio-focus");
        $(this).addClass("radio-no-focus");
    });
};

function styleCheckboxes() {
    $(".checkbox").addClass("checkbox-no-focus");

    $(".checkbox").focusin(function () {
        $(this).addClass("checkbox-focus");
        $(this).removeClass("checkbox-no-focus");
    });
    $(".checkbox").focusout(function () {
        $(this).removeClass("checkbox-focus");
        $(this).addClass("checkbox-no-focus");
    });
};