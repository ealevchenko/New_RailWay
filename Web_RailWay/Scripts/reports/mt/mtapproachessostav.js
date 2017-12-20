$(document).ready(function () {
    //// Обнулим сокеты
    //$.cookie('view-cars', '0');

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
            },
        }).bind('datepicker-closed', function () {
            $("#report-menu").empty();
            // Очистить operation
            $("#report-operation").empty();
            // Очистить operation-detali 
            $("#report-operation-content").empty();
            $('form#fmList').submit(); // Отправить форму
        });
    // Задать дату 
    var dt = new Date();
    var d_start = new Date(dt.getFullYear(), dt.getMonth(), (dt.getDate()-1));
    var d_stop = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 23, 59, 59);
    var s_d_start = d_start.getDate() + '.' + (d_start.getMonth()+1) + '.' + d_start.getFullYear() + ' ' + d_start.getHours() + ':' + d_start.getMinutes();
    var s_d_stop = d_stop.getDate() + '.' + (d_stop.getMonth()+1) + '.' + d_stop.getFullYear()+' '+d_stop.getHours()+':'+d_stop.getMinutes()
    $('#select-range').data('dateRangePicker').setDateRange(s_d_start, s_d_stop);
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

    //Первая выборка
    OnBegin();
    $.ajax({
        url: '/railway/MTApproaches/ListSostav/',
        type: 'GET',
        data: { date_start: d_start.toISOString(), date_stop: d_stop.toISOString(), },
        dataType: 'html',
        success: function (data) {
            selectPeriod(data);
        },
        error: function (x, y, z) {
            LockScreenOff();
            alert(x + '\n' + y + '\n' + z);
        }
    });


});

function selectPeriod(data) {
    // Очистить 
    $("#report-location").empty();
    // Очистить 
    $("#report-location-content").empty();
    // Показать составы 
    var target = $("#report-menu");
    target.empty();
    target.append(data);
    LockScreenOff();

    $(function () {
        $('a[name ="link-operation"]').click(function (evt) {
            evt.preventDefault();
            OnBegin();
            $('a[name ="link-operation"]').removeClass();
            $(this).addClass('selected');
            var id_sostav = $(this).attr("id")
            // Получим движение состава
            $.ajax({
                url: '/railway/MTApproaches/ListHistoryLocation/',
                type: 'GET',
                data: { id_sostav: id_sostav, route: false },
                dataType: 'html',
                success: function (data) {
                    listLocationSostav(data);
                },
                error: function (x, y, z) {
                    LockScreenOff();
                    alert(x + '\n' + y + '\n' + z);
                }
            });
        });
    })

    $(document).ready(function () {
        var el = $('a[name ="link-operation"]').first();
        if (el != null) {
            el.click();
        }
    });

}

function listLocationSostav(data) {
    // Очистить operation-detali 
    $("#report-location-content").empty();
    // Показать операции 
    var target = $("#report-location");
    target.empty();
    target.append(data);
    LockScreenOff();

    $(function () {
        $('a[name ="link-sostav"]').click(function (evt) {
            evt.preventDefault();
            OnBegin();
            $('a[name ="link-sostav"]').removeClass();
            $(this).addClass('selected');
            var id_sostav = $(this).attr("id")
            // Получим движение состава
            $.ajax({
                url: '/railway/MTApproaches/SostavDetali/',
                type: 'GET',
                data: { id_sostav: id_sostav },
                dataType: 'html',
                success: function (data) {
                    detaliSostav(data);
                },
                error: function (x, y, z) {
                    LockScreenOff();
                    alert(x + '\n' + y + '\n' + z);
                }
            });
        });
    })

    $(document).ready(function () {
        var el = $('a[name ="link-sostav"]').first();
        if (el != null) {
            el.click();
        }
    });
}

function detaliSostav(data) {
    var target = $("#report-location-content");
    target.empty();
    target.append(data);
    LockScreenOff();
}



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


