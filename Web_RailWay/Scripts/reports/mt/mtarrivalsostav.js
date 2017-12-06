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

function selectPeriod(data) {

    LockScreenOff();
    var target = $("#report-menu");
    target.empty();
    target.append(data);
}

function OnBegin() {
    LockScreen('Мы обрабатываем ваш запрос...');
}

function OnFailure(request, error) {
    //alert("This is the OnFailure Callback:" + error);
}

function OnComplete(request, status) {
    //alert("This is the OnComplete Callback: " + status);        
}