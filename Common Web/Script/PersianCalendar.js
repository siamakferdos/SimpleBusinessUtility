
//////////////////////////////////// تقویم  /////////////////////////////////////////////////////////////////////////
var dateNow = GetServerDate(); // = new Date();
var dateNowYear = 0;
var dateNowMonth = 0;
var GSFirstTemp = 0;
var monthlengthNo = 0;
var flagclick = false;
//array for length months
var monthlength = new Array();
monthlength = [31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29];

var year = 1391;
//array for monthsNames shamsi
var monthsNames = new Array();
monthsNames[0] = "فروردین";
monthsNames[1] = "اردیبهشت";
monthsNames[2] = "خرداد";
monthsNames[3] = "تیر";
monthsNames[4] = "مرداد";
monthsNames[5] = "شهریور";
monthsNames[6] = "مهر";
monthsNames[7] = "آبان";
monthsNames[8] = "آذر";
monthsNames[9] = "دی";
monthsNames[10] = "بهمن";
monthsNames[11] = "اسفند";

//call create datepicker function
function datePickerFunc(element) {
    $('div.datePickerload').remove();
    //dateNow = GetServerDate();
    var target = $(element).parent().find("input[data-prg-date='datePicker']");  //  $("input[data-prg-date='datePicker']");// $(".datePicker");
    var DatePickerContainerAmount = $(element).parent().find("input[data-prg-date='datePicker']").val();  // $("input[data-prg-date='datePicker']").val();
    var year = DatePickerContainerAmount.substr(0, 4);
    var mounth = DatePickerContainerAmount.substr(5, 2);
    var day = DatePickerContainerAmount.substr(8, 2);
    monthlengthNo = monthlength[mounth - 1];
    var mounthDes = mounth - 1;
    var getFirst = getFirstDay(dateNow.getYear(), dateNow.getMonth());
    GSFirstTemp = getFirst;
    day_no = GSFirstTemp; //+ " .btn"
    var $div = $("<div>", { class: "datePickerload" });
    $(element).parent().append($div);

    var DatePickerContainerDiv = "";
    if ($(element).attr("data-prg-datepickeryh")) {
        DatePickerContainerDiv = "<div id='DatePickerContainer' style='position: absolute; width: auto; height: auto; z-index: 500; '>" +
            "<table id='DatePickerContainerTable' class='ng-isolate-scope'><thead><tr>" +
            "<th><button id='yearInc' class='btn btn-default btn-sm pull-left' type='button' onclick='yearIncclick()'><i class='glyphicon glyphicon-chevron-right'></i></button></th>" +
            "<th colspan='5'><button id='yearVal' value='" + year + "' class='btn btn-default btn-sm btn-block' type='button'><strong class='ng-binding'>" + year + "</strong></button></th>" +
            "<th><button id='yearDec' class='btn btn-default btn-sm pull-right' type='button' onclick='yearDecclick();'><i class='glyphicon glyphicon-chevron-left'></i></button></th>" +
            "</tr><tr>" +
            "<th><button id='monthInc' class='btn btn-default btn-sm pull-left' type='button' onclick='monthIncclick();'><i class='glyphicon glyphicon-chevron-right'></i></button></th>" +
            "<th colspan='5'><button id='monthVal' value='" + mounthDes + "' class='btn btn-default btn-sm btn-block' type='button'><strong class='ng-binding'>" + monthsNames[mounth - 1] + "</strong></button></th>" +
            "<th><button id='monthDec' class='btn btn-default btn-sm pull-right' type='button' onclick='monthDecclick();'><i class=' glyphicon glyphicon-chevron-left'></i></button></th>" +
            "</tr></thead><tbody></tbody>" +
            "</table></div>";
    } else {
        DatePickerContainerDiv = "<div id='DatePickerContainer' style='position: absolute; width: auto; height: auto; z-index: 500; '>" +
           "<table id='DatePickerContainerTable' class='ng-isolate-scope'><thead>" +
           "<tr><th><button id='monthInc' class='btn btn-default btn-sm pull-left' type='button' onclick='monthIncclick();'><i class='glyphicon glyphicon-chevron-right'></i></button></th>" +
           "<th colspan='3'>" +
           "<button id='monthVal' value='" + mounthDes + "' class='btn btn-default btn-sm btn-block text-smal text-bold' type='button'><strong class='ng-binding'>" + monthsNames[mounth - 1] + "</strong></button>" +
           "</th><th colspan='2'>" +
           "<button id='yearVal' value='" + year + "' class='btn btn-default btn-sm btn-block text-smal text-bold' type='button'><strong class='ng-binding'>" + year + "</strong></button></th>" +
           "<th><button id='monthDec' class='btn btn-default btn-sm pull-right' type='button' onclick='monthDecclick();'><i class=' glyphicon glyphicon-chevron-left'></i></button></th>" +
           "</tr></thead><tbody></tbody>" +
           "</table></div>";
    }


    $(element).parent().find("div[class='datePickerload']").append(DatePickerContainerDiv);    
    CreateDayForMonth(monthlengthNo); 
    $("#day" + parseInt(parseInt(day) + parseInt(day_no) - 1)).addClass("SelcetDayBtn");
    $(element).parent().find("input[data-prg-date='datePicker']").addClass("activeDataPicker");
    flagclick = true;
}

//افزایش سال
//$("#yearInc").click(function () {
function yearIncclick() {
    for (var i = 0; i < 12; i++) {
        $("#monthInc").trigger("click");
    }
    flagclick = true;
}
//});

//کاهش سال
//$("#yearDec").click(function () {
function yearDecclick() {
    for (var i = 0; i < 12; i++) {
        $("#monthDec").trigger("click");
    }
    flagclick = true;
}
//});

//افزایش ماه
//$("#monthInc").click(function () {
function monthIncclick() {
    var temp = parseInt($("#monthVal").val()) + 1;
    GSFirstTemp = (monthlengthNo % 7) + GSFirstTemp;
    if (GSFirstTemp > 7)
        GSFirstTemp = GSFirstTemp - 7;
    if (temp == 12) temp = 0;
    monthlengthNo = monthlength[temp];

    if (temp == 11 && Kabiseyear(parseInt($("#yearVal").text())))
        ++monthlengthNo;

    if (temp == 0)
        $("#yearVal").text(parseInt($("#yearVal").text()) + 1);

    CreateDayForMonth(monthlengthNo);
    $("#monthVal").text(monthsNames[temp]);
    $("#monthVal").val(temp);
    flagclick = true;
}
//});

//کاهش ماه
//$("#monthDec").click(function () {
function monthDecclick() {
    var temp = parseInt($("#monthVal").val()) - 1;
    if (temp == -1) temp = 11;
    monthlengthNo = monthlength[temp];

    if (temp == 11 && Kabiseyear(parseInt($("#yearVal").text()) - 1))
        ++monthlengthNo;

    GSFirstTemp = 7 - (((7 - GSFirstTemp + 1) + monthlengthNo) % 7) + 1;

    if (temp == 11)
        $("#yearVal").text(parseInt($("#yearVal").text()) - 1);

    CreateDayForMonth(monthlengthNo);
    $("#monthVal").text(monthsNames[temp]);
    $("#monthVal").val(temp);
    flagclick = true;
}
//});

//انتخاب تاریخ
//$("#DatePickerContainer tbody td").click(function () {
function DatePickerContainerTdclick(element) {
    var month = parseInt($("#monthVal").val()) + 1;
    //var day = $(this).text();  //.find(":button")
    var day = $(element).text();
    if (month < 10) month = "0" + month;
    if (day < 10) day = "0" + day;
    $(".activeDataPicker").val($("#yearVal").text() + "/" + month + "/" + day);
    $(".activeDataPicker").removeClass("activeDataPicker");
    //$("#day" + day).addClass("SelcetDayBtn").siblings('.ng-isolate-scope .ng-scope').removeClass('SelcetDayBtn');  //tr??
    //$("#DatePickerContainer").fadeOut();
    // $(".datePickerload").empty();
    $('div.datePickerload').remove();
    flagclick = true;
}
//});

//مشخص کردن اولین روز ماه
function getFirstDay(theYear, theMonth) {
    var firstDate = new Date(theYear, theMonth, 1)
    return firstDate.getDay()
}

//تابع ایجاد سلول های -روز ها- ماه
function CreateDayForMonth(monthlengthNo) {
    var body = "";
    body += "<tr ng-repeat='row in rows' class='ng-scope'>" +
        "<td class='text-center ng-scope text-smal text-bold DatePickerContainerth'>ش</td><td class='text-center ng-scope text-smal text-bold DatePickerContainerth'>ی</td><td class='text-center ng-scope text-smal text-bold DatePickerContainerth'>د</td>" +
        "<td class='text-center ng-scope text-smal text-bold DatePickerContainerth'>س</td><td class='text-center ng-scope text-smal text-bold DatePickerContainerth'>چ</td><td class='text-center ng-scope text-smal text-bold DatePickerContainerth'>پ</td><td class='text-center ng-scope text-smal text-bold DatePickerContainerth'>ج</td>";
    body += "</tr>";

    for (var i = 1, k = 1; i <= 6; i++) {
        body += "<tr ng-repeat='row in rows' class='ng-scope'>";

        for (j = 1; j <= 7; j++, k++) {
            //if (rowNumber == 1 && skipDays > j)
            //    body += "<td id='day' class='text-center ng-scope' ng-repeat='dt in row'></td>";
            //else {
            body += "<td id='day" + k + "' class='text-center ng-scope DatePickerContainertd' ng-repeat='dt in row' onclick='DatePickerContainerTdclick(this);' ></td>";
            //day++;
            //}
        }
        body += "</tr>";
    }
    $(".ng-isolate-scope tbody").html(body);

    for (var i = GSFirstTemp, j = 1 ; i < GSFirstTemp + monthlengthNo; i++, j++) {
        $("#day" + i).text(j);
        $("#day" + i).val(j);
    }

    for (var i = 0 ; i < GSFirstTemp; i++) {
        $("#day" + i).removeClass("DatePickerContainertd");
    }
    for (var i = GSFirstTemp + monthlengthNo ; i <= 42; i++) {
        $("#day" + i).removeClass("DatePickerContainertd");
    }

}
//تاریخ میلادی سرور
function GetServerDate() {
    var req = new XMLHttpRequest();
    req.open('GET', document.location, false);
    req.send(null);
    return new Date(req.getResponseHeader("date")); //;.getDay();
}
//تست کبیسه بودن سال
function Kabiseyear(year) {
    var a = 0.025;
    var b = 266;
    var leapDays0;
    var leapDays1;
    if (year > 0) {
        leapDays0 = ((parseInt(year) + 38) % 2820) * 0.24219 + parseFloat(a);
        leapDays1 = ((parseInt(year) + 39) % 2820) * 0.24219 + parseFloat(a);
    }
    else if (year < 0) {
        leapDays0 = ((parseInt(year) + 39) % 2820) * 0.24219 + parseFloat(a);
        leapDays1 = ((parseInt(year) + 40) % 2820) * 0.24219 + parseFloat(a);
    }
    else
        return false;

    var frac0 = parseInt((leapDays0 - parseInt(leapDays0)) * 1000);
    var frac1 = parseInt((leapDays1 - parseInt(leapDays1)) * 1000);

    if (frac0 <= b && frac1 > b)
        return true;
    else
        return false;
}
//کنترل باز و بسته شدن کنترل تاریخ
$(document).click(function () {
    if (flagclick != true) {
        $(".activeDataPicker").removeClass("activeDataPicker");
        //$("#DatePickerContainer").fadeOut();
        //$(".datePickerload").empty();
        $('div.datePickerload').remove();
    }
    flagclick = false;
});

