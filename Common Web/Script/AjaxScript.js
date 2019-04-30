$(function () {


    // Changes panel while radio button selected index changes
    // Conditions: The radio buttons must hold in div with attribute data-prg-type='radioTab'
    // Act: Depend on which radio button index is cheched The equvalant div with attribute [data-prg-modal='radioTab'] display and the others will hide
    $("div[data-prg-type='radioTab']  input[type='radio']").on("click", function () {
        var selectedRadioIndex = $(this).parent().parent().find("input[type = 'radio']").index(this) + 3;
        $(this).parent().parent().parent().children("div[data-prg-modal='radioTab']").hide();
        var tabPath = "div[data-prg-modal='radioTab']:nth-child(" + selectedRadioIndex + ")";
        $(this).parent().parent().parent().find(tabPath).css("display", "block");
    });



    // Displays Popup window with ajax content
    // condition : The input button or button must have attributes data-prg-popup='true' 
    //             , "data-prg-ajax" : The value of this attibute indicate the address of action which would display as a popup content
    //             , "data-prg-ajaxfinaltarget" : The value of this attribute indicate the element which should hold the popup buttons ajax result
    // Act: The popup display and Related ajax content will display on it
    //$("input[data-prg-popup='true'],button[data-prg-popup='true']").on("click", function () {
    $(document).on("click", "input[data-prg-popup='true'],button[data-prg-popup='true']", function () {
        var thisElement = $(this);
        var elementFunction = $(this).attr("data-prg-ajax");

        //var finalTarget = "";
        if (thisElement.attr("data-prg-ajaxfinaltarget") != "") {
            $("input[data-prg-type='targetSelector']").val(thisElement.attr("data-prg-ajaxfinaltarget"));
            //finalTarget = "?popupTarget=" + thisElement.attr("data-prg-ajaxfinaltarget");
        }

        $("div[data-prg-modal='popup']").slideDown();
        $("div[data-prg-type='backdrop']").show();
        if (elementFunction.length > 0) {
            var target = $("#PartialContainer");
            var url = "/" + elementFunction; // + finalTarget; //InsertAssociatedOrganization"
            DoAjax(url, target, "");
        }

        //   var ret = eval(elementFunction + "();");

        $("div[data-prg-modal='popup']").find("button[data-prg-type='popupSave']").on("click", function () {
            $("div[data-prg-modal='popup']").slideUp();
            $("div[data-prg-type='backdrop']").fadeOut(250);
            $("#PartialContainer").html("");
        });
    });

    // A Json Popup Displays Popup window with ajax content, the popup has model which action recieves as parameter
    // condition : The input button or button must have attributes data-prg-popup='json' 
    //             , "data-prg-modelField" : the value of this attibute is a selector of hidden element which holds model 
    //             , "data-prg-ajax" : The value of this attibute indicate the address of action which would display as a popup content
    //             , "data-prg-ajaxfinaltarget" : The value of this attribute indicate the element which should hold the popup buttons ajax result
    // Act: The popup display and Related ajax content will display on it
    $("input[data-prg-popup='json'],button[data-prg-popup='json']").on("click", function () {

        var thisElement = $(this);
        var elementFunction = $(this).attr("data-prg-ajax");
        var data = $("#" + $(this).attr("data-prg-modelField")).val();
        var finalTarget = "";
        if (thisElement.attr("data-prg-ajaxfinaltarget") != "") {
            $("input[data-prg-type='targetSelector']").val(thisElement.attr("data-prg-ajaxfinaltarget"));
            //finalTarget = "?popupTarget=" + thisElement.attr("data-prg-ajaxfinaltarget");
        }

        $("div[data-prg-modal='popup']").slideDown();
        $("div[data-prg-type='backdrop']").show();
        if (elementFunction.length > 0) {
            var target = $("#PartialContainer");
            var url = "/" + elementFunction + finalTarget; //InsertAssociatedOrganization"
            var t = [];
            t[0] = target;
            DoJsonAjax(url, t, data, "");
        }

        //   var ret = eval(elementFunction + "();");

        $("div[data-prg-modal='popup']").find("button[data-prg-type='popupSave']").on("click", function () {
            $("div[data-prg-modal='popup']").slideUp();
            $("div[data-prg-type='backdrop']").fadeOut(250);
            $("#PartialContainer").html();
        });
    });

    // Popup Hides When button is clicked
    // Condition : button should have attribute data-prg-type='popupCancel' 
    // Popup Slowly Disapear
    $("div[data-prg-modal='popup']").find("button[data-prg-type='popupCancel']").on("click", function () {
        $("div[data-prg-modal='popup']").slideUp();
        $("div[data-prg-type='backdrop']").fadeOut(250);
        $("#PartialContainer").html();
    });

    $("form[data-prg-ajax='GlobalAjax']").on("submit", function (e) {
        e.preventDefault();
        if ($(this).valid()) {
            var form = $(this);
            var url = form.attr("action");
            var data = form.serialize();

            var targets = $(form.attr("data-prg-target"));
            var globalFunctionName = $(form.attr("data-prg-globalFunctionName"));
            DoGlobalAjax(URL, data, targets, globalFunctionName);
        }
    });


    // Ajax Form Submit
    // condition : the Form should have attributes data-prg-ajax='true',
    //             ,"data-prg-target" : the value of this attibute indicate element selector which display ajax result
    // Act : The form Submited and all inputs value will clear

    $("form[data-prg-ajax='true']").on("submit", function (e) {
        e.preventDefault();
        if ($(this).valid()) {

            var $form = $(this);
            var options = {
                url: $form.attr("action"),
                type: $form.attr("method"),
                data: $form.serialize()
            };

            var target = $($form.attr("data-prg-target"));
            $.ajax({
                url: $form.attr("action"),
                type: $form.attr("method"),
                data: $form.serialize(),
                success: function (data) {
                    target.html(data);
                    clearFormElements($form);
                    //$('html, body').animate({
                    //    scrollTop: $form.offset().top
                    //}, 2000);
                    $form.prepend("<div>اطلاعات با موفقیت درج گردید</div>");
                },
                error: function (data) {
                    target.html('<span class="text-danger"><i class="fa fa-lg fa-warning margin-left"></i>ارتباط بر قرار نشد!</span>' + enableInput);

                }
            });
        }
    });

    // Json Form Submit which manages multi Returned value
    // condition : the Form should have attributes data-prg-ajax='multiTarget',
    //             ,"data-prg-target" : the value of this attibute indicate varous elements selectors, seperated by "|" and hold return value in order
    //             ,data-prg-clearElements : the value of this attribute holds various element selectors, seperated by "|" which should cleared after successfull form submit
    // Act : The form Submited and each returned data display in equvalant target and all inputs value will clear

    $("form[data-prg-ajax='multiTarget']").on("submit", function (e) {
        e.preventDefault();
        if ($(this).valid()) {
            var $form = $(this);
            var clearElements = $form.attr("data-prg-clearElements").split("|");
            var target = $form.attr("data-prg-target").split("|");
            var additionalParameter = "";

            if ($(this).is("[aditionalParameter]"))
                if ($(this).attr("aditionalParameter") != null)
                    additionalParameter = $(this).attr("aditionalParameter");
            //var enableInput = "";
            //if (disabledInput != "") {
            //    enableInput = "<script>$('#" + disabledInput + "').prop('disabled', false);<\/script>"
            //}   
            //alert($form.attr("action") + $(this).attr("aditionalParameter"));
            $(target[0]).html("<img src='/Images/ajax-loader.gif' class='ajax-Loader' />");
            $.ajax({
                url: $form.attr("action") + additionalParameter,
                type: $form.attr("method"),
                data: $form.serialize(),
                dataType: "json",
                error: function (data) {
                    $(target[0]).html('<span class="text-danger"><i class="fa fa-lg fa-warning margin-left"></i>ارتباط بر قرار نشد!</span>');
                },
                success: function (data) {
                    var count = 0;
                    if (data["error"]) {
                        $(target[0]).html(data["error"]);
                    }
                    else {
                        clearFormElements($form);
                        $.each(data, function (key, val) {
                            $(target[count]).html(val);
                            count++;
                        });
                        count = 0;
                        $.each(clearElements, function () {
                            $(this.toString()).html("");
                        });
                    }
                }
            });
        }

    });



    // Checking if pressed key is shortcut key or not
    // Condition : key must pressed in input element which has sibling button with attribute "data-prg-shortcut"
    // Act : Relevent Buttons click event will trigger
    $("input").keydown(function (e) {

        var pkey = e.keyCode;
        var target;
        if (pkey == 13) {
            // key = enter
            target = $(this).parent().find("button[data-prg-shortcut='enter'],input[data-prg-shortcut='enter']");
            target.trigger("click");
            e.preventDefault();
        }
        if (pkey == 112) {
            // key = F1
            target = $(this).parent().find("button[data-prg-shortcut='f1'],input[data-prg-shortcut='f1']");
            target.trigger("click");
        }
        if (pkey == 27) {
            // key = Escape
            target = $("body").find("button[data-prg-shortcut='escape']");
            if (target == null) {
                target = $(this).parent().parent().find("button[data-prg-shortcut='escape']");
            }
            target.trigger("click");
        }

        if (pkey == 9) {
            // key = Tab
            target = $(this).parent().find("button[data-prg-shortcut='tab']");
        }
    });

    // Closing Popup window
    // Condition : The body should have atleast one button with "data-prg-shortcut"="escape" 
    // Act : Relevent Buttons click event will trigger
    $("body").keydown(function (e) {
        var pkey = e.keyCode;
        if (pkey == 27) {
            var target = $("body").find("button[data-prg-shortcut='escape']");
            target.trigger("click");
        }
    });


});


// function that send asynchronous request to server
// Parameters : Url    : The Address of Action plus query strings if is needed
//              target : The jquery selector of target elemet which ajax result should display on it
//              DisableInput : the jaquery selector of element which should be disabled during processing ajax request
function DoAjax(url, target, disabledInput) {
    target.html("<img src='/Images/ajax-loader.gif' class='ajax-Loader' />");
    var enableInput = "";
    if (disabledInput != "") {
        enableInput = "<script>$('#" + disabledInput + "').prop('disabled', false);<\/script>";
    }
    $.ajax({
        url: url,
        error: function (data) {
            alert(data.responseText);
            target.html('<span class="text-danger"><i class="fa fa-lg fa-warning margin-left"></i>ارتباط بر قرار نشد!</span>' + enableInput);
        },
        success: function (data) {
            target.html(data + enableInput);
        }
    });
}

// function that send asynchronous request to server and diplay result in multiple targets
// Parameters : Url    : The Address of Action plus query strings if is needed
//              target : the array of target elemets jquery selector which ajax result should take palce on them
//              DisableInput : the jaquery selector of element which should be disabled during processing ajax request
function DoMultiTargetAjax(url, target, disabledInput) {

    var enableInput = "";
    if (disabledInput != "") {
        enableInput = "<script>$('#" + disabledInput + "').prop('disabled', false);<\/script>";
    }

    target[0].html("<img src='/Images/ajax-loader.gif' class='ajax-Loader' />");
    //alert(target[0] + target[1]);
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: function (data) {
            target[0].html(data.statusText);

            //target[0].html('<span class="text-danger"><i class="fa fa-lg fa-warning margin-left"></i>ارتباط بر قرار نشد!</span>' + enableInput);
        },
        success: function (data) {
            target[0].html(enableInput);
            var count = 0;
            $.each(data, function (key, val) {
                if (val != "NoUpdate") {
                    if (target[count].prop("tagName") == "INPUT" || target[count].prop("tagName") == "SELECT")
                        if (target[count].prop("type") == "checkbox")
                            target[count].prop("checked", val);
                        else
                            target[count].attr("value", val);
                    else
                        target[count].html(val);
                }
                count++;
            });
        }
    });
}

// the function which clear all form inputs
function clearFormElements(frm) {
    frm.find(':input').each(function () {
        switch (this.type) {
            case 'password':
                //case 'select-multiple':
                //case 'select-one':
            case 'text':
            case 'textarea':
                $(this).val('');
                break;
                //case 'checkbox':
                //case 'radio':
                //    this.checked = false;
        }
    });
}

// function that send asynchronous json request to server and diplay result in multiple targets
// Parameters : Url    : The Address of Action plus query strings if is needed
//              target : the array of target elemets jquery selector which ajax result should take palce on them
//              data : the jason data containing model of page which should sended to server
//              DisableInput : the jaquery selector of element which should be disabled during processing ajax request
function DoJsonAjax(url, target, data, disabledInput) {
    var enableInput = "";
    if (disabledInput != "") {
        enableInput = "<script>$('#" + disabledInput + "').prop('disabled', false);<\/script>";
    }
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        error: function (data1) {
            target[0].html(data1.statusText);
            //$(target[0]).html('<span class="text-danger"><i class="fa fa-lg fa-warning margin-left"></i>ارتباط بر قرار نشد!</span>' + enableInput);
        },
        success: function (data1) {
            var count = 0;
            $.each(data1, function (key, val) {
                if (count == 0)
                    $(target[count++]).html(val + enableInput);
                else
                    $(target[count++]).html(val);
            });
            count = 0;
            $.each(clearElements, function () {
                $(this.toString()).html("");
            });
        }
    });
}



function DoGlobalAjax(url, inputData, targets, generalFuntionName, shouldLockScreen) {

    var _inputData = "";
    if (inputData != null)
        _inputData = inputData;
    if (shouldLockScreen)
        $("body").prepend("<div id='ajaxRunning' ><div style='width:100%; height:100%; z-index:99999; position:fixed; opacity:0.4; " +
            " background-color:black;' class='text-center'>" +
            "</div><div style='position: fixed; z-index: 999999; right: 50%; top: 50%;' >" +
            "<img src='/Images/ajax-loader.gif' class='text-center' width='80' height='80' /></div></div>");
    $.ajax({
        url: url,
        type: "POST",
        data: _inputData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        error: function (data1) {
            targets["error"].html(data1.statusText);
        },
        success: function (data) {
            if (data.hasOwnProperty("Data")) {
                data = JSON.parse(data.Data);
            }

            $.each(data, function (key, val) {
                if (key == "json1") {
                    targets.html(val);
                }
                else if (key == "html") {
                    $.each(JSON.parse(data[key]), function (k, v) {
                        if (targets[k].is("input") || targets[k].is("select"))
                            if (targets[k].prop("type") == "checkbox")
                                targets[k].prop("checked", v);
                            else
                                targets[k].attr("value", v);
                        else {
                            targets[k].html(v);
                        }
                    });
                }
            });
            if (data.hasOwnProperty("js")) {
                eval(data.Js);
            }
            if (generalFuntionName) {
                if (typeof generalFuntionName != "string")
                    if (data.hasOwnProperty("data"))
                        generalFuntionName(data.data);
                    else {
                        generalFuntionName();
                    }
                else {
                    if (data.hasOwnProperty("data"))
                        window["functionName"](data.data);
                    else {
                        window["functionName"]();
                    }
                }
            }
        },
        complete: function () {
            if (shouldLockScreen)
                $("body").find("div[id='ajaxRunning']").remove();
        }
    });
}











