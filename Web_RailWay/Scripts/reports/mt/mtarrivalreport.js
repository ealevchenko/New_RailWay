$(document).ready(function () {

    myVar = $.cookie('lang');

    $('#select-range').dateRangePicker(
    {
        startOfWeek: 'monday',
        separator: myVar == 'en' ? 'to' : 'по',
        language: myVar,
        format: myVar == 'en' ? 'MM/DD/YYYY HH:mm' : 'DD.MM.YYYY HH:mm',
        autoClose: false,
        showShortcuts: false,
        getValue: function () {
            if ($('#date-start').val() && $('#date-stop').val())
                return $('#date-start').val() + ' to ' + $('#date-stop').val();
            else
                return '';
        },
        setValue: function (s, s1, s2) {
            $('#date-start').val(s1);
            $('#date-stop').val(s2);
        },
        time: {
            enabled: true
        }
    }).bind('datepicker-closed', function () {
            $("#report-menu").empty();
            // Очистить operation
            $("#report-operation").empty();
            // Очистить operation-detali 
            $("#report-operation-content").empty();
                $('form#fmList').submit(); // Отправить форму  
    });

    //валидация
    $(function () {

        $('form#fmList').validate({
            highlight: function (element, errorClass) {
                $(element).add($(element).parent()).addClass("invalidElem");
                $(element.form).find("div[for=" + element.id + "]").addClass(element.id);
            },
            unhighlight: function (element, errorClass) {
                $(element).add($(element).parent()).removeClass("invalidElem");
                $(element.form).find("div[for=" + element.id + "]").removeClass(errorClass);
            },
            //errorElement: "div",
            errorClass: "errorMsg"
        });

        $.validator.addMethod("mydate", function (value, element) {
            return /^[0-9]{1,2}[\/\.][0-9]{1,2}[\/\.][0-9]{4}[/ ][0-9]{1,2}[\:][0-9]{1,2}$/i.test(value);
        }, myVar == 'en' ? "Enter the correct date (mm/dd/yyyy hh:mm)" : "Введите правильно дату (дд.мм.гггг чч:мм)")

        $.validator.addClassRules({
            dateValidation: {
                mydate: true,
                required: true
            },
            messages: {
                dateValidation: {
                    required: "Поле с датой пустое!",
                }
            },


        })

        $('input#date-start').addClass("dateValidation").change(function (e) {
            $('form#fmList').validate().element($(e.target));
        });

        $('input#date-stop').addClass("dateValidation").change(function (e) {
            $('form#fmList').validate().element($(e.target));
        });

    });

});

//function listNotArrival(data) {

//    // Показать составы 
//    var target = $("#list-not-arrival");
//    target.empty();
//    target.append(data);
//}

function selectPeriod(data) {
    var target = $("#report-detali");
    target.empty();
    target.append(data);

//        // Открывающиеся панели
//        var list_not_arrival = $.cookie('list-not-arrival');
//        $('div#list-not-arrival').attr('style', list_not_arrival);
//        $('a.show-detali-object').click(function (evt) {
//            evt.preventDefault();
//            $(this).siblings('.detali-object').slideToggle();
//            var id = $(this).siblings('.detali-object').attr("id");
//            var style = $.cookie(id);
//            if (style == null | style == 'display: none;') {
//                $.cookie(id, 'display: block;');
//            } else { $.cookie(id, 'display: none;'); }
//        });
}

//function detaliSostavOperation(data) {
//    //LockScreenOff();
//    // Отметить выбранную ссылку 
//    $(function () {
//        $('a#a-select-cars').click(function (evt) {
//            $('a#a-select-cars').removeClass();
//            $(this).addClass('selected');
//        });
//    })

//    var target = $("#report-operation-content");
//    target.empty();
//    target.append(data);
//}

function OnBegin() {
    myVar = $.cookie('lang');
    LockScreen(myVar == 'en' ? 'We are processing your request ...' : 'Мы обрабатываем ваш запрос...');
}

function OnFailure(request, error) {
    //alert("This is the OnFailure Callback:" + error);
    LockScreenOff();
    alert("Ошибка: " + error);
}

function OnComplete(request, status) {
    //alert("This is the OnComplete Callback: " + status);    
    LockScreenOff();
}