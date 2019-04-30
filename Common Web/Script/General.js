
//بدست آوردن مقدار یک کلید در یک رشته 
function GetValue(searchKey, searchcontent, keyValueSeparator, pairSeparator) {

    var i1 = searchcontent.indexOf(searchKey);
    var i2 = searchcontent.indexOf(keyValueSeparator, i1);
    var i3 = searchcontent.indexOf(pairSeparator, i2);
    var result = searchcontent.slice(i2 + 1, i3);

    return result;
}

//غیر فعال و فعال کردن دکمه جستجو برای کلماتی با کمتر از 4 حرف
function SetSearchButtonEnabling(selector) {
    var n = $(selector).attr("data-prg-Length");
    var selectorval = $(selector).val();
    if (selectorval.length < (n - 1)) {
        $(selector).next("input[type='button'], button").prop("disabled", true); // .css("enable", "false");
        $(".text-danger").html("<span class='text-warning'><i class='fa fa-lg fa-warning  margin-left'></i>برای جستجو تایپ حداقل " + n + " کاراکتر الزامیست</span>");
    } else {
        $(selector).next("input[type='button'], button").prop("disabled", false); // .css("enable", "true");
        $(".text-danger").empty();
    }
}