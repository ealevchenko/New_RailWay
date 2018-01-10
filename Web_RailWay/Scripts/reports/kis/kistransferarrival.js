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
    var d_start = new Date(dt.getFullYear(), dt.getMonth(), (dt.getDate() - 1));
    var d_stop = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 23, 59, 59);
    var s_d_start = d_start.getDate() + '.' + (d_start.getMonth() + 1) + '.' + d_start.getFullYear() + ' ' + d_start.getHours() + ':' + d_start.getMinutes();
    var s_d_stop = d_stop.getDate() + '.' + (d_stop.getMonth() + 1) + '.' + d_stop.getFullYear() + ' ' + d_stop.getHours() + ':' + d_stop.getMinutes()
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
        url: '/railway/KIST/ListBufferArrivalSostavOfDate/',
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
    var target = $("#report-table");
    target.empty();
    target.append(data);
    LockScreenOff();

    myVar = $.cookie('lang');

    var table = $('#list-buffer-arrival-sostav').DataTable({
        "paging": false,
        "ordering": true,
        "info": false,
        language: {
            decimal: myVar == 'en' ? "." : ",",
            search: myVar == 'en' ? "Search" : "Найти",
        },
        columns: [
            
            null,
            null,
            { "orderData": [2,1] },
            null,
            null,
            { "orderable": false },
            { "orderable": false },
            null,
            null,
        ],
        initComplete: function () {
            this.api().columns([2, 7]).every(function () {
                var column = this;
                var name = $(column.header()).attr('title');
                var select = $('<select><option value="">' + (myVar == 'en' ? 'All' : 'Все') + '</option></select>')
                    .appendTo($(column.header()).empty().append(name))
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        },
        jQueryUI: true,
    });
    // Отсортируем по времени по убыванию
    table.order( [ 1, 'desc' ] )
    table.draw();
    // Обработчик кнопки закрыть
    $('input[name ="close-sostav"]').click(function (evt) {
        evt.preventDefault();
        OnBegin();
        var id = $(this).attr("id")
        // Закроем состав
        $.ajax({
            url: '/railway/KIST/CloseBufferArrivalSostav/',
            type: 'POST',
            data: { id: id },
            dataType: 'html',
            success: function (data) {
                //alert(data)
                // Получим изменение по закрытому составу
                $.ajax({
                    url: '/railway/KIST/GetCloseBufferArrivalSostav/',
                    type: 'GET',
                    data: { id: id },
                    dataType: 'html',
                    success: function (data) {
                        var target = $('td[name ="button-close-' + id + '"]');
                        target.empty();
                        target.append(data);
                        LockScreenOff();
                    },
                    error: function (x, y, z) {
                        LockScreenOff();
                        alert(x + '\n' + y + '\n' + z);
                    }
                });
            },
            error: function (x, y, z) {
                LockScreenOff();
                alert(x + '\n' + y + '\n' + z);
            }
        });
    });
    // Обработчик выбора строки таблицы
    $('#list-buffer-arrival-sostav tr[name="bas"]').click(function (evt) {
            evt.preventDefault();
            //OnBegin();
            $('#list-buffer-arrival-sostav tr[name="bas"]').removeClass('selected');
            $(this).addClass('selected');
            var id = $(this).attr("id")
            // Получим список вагонов и состояние переноса
            $.ajax({
                url: '/railway/KIST/ListCarsBufferArrivalSostav/',
                type: 'GET',
                data: { id: id },
                dataType: 'html',
                success: function (data) {
                    // Показать операции 
                    var target = $("#report-detali");
                    target.empty();
                    target.append(data);
                },
                error: function (x, y, z) {
                    LockScreenOff();
                    alert(x + '\n' + y + '\n' + z);
                }
            });
        });
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


