$(document).ready(function () {

    myVar = $.cookie('lang');

    // заполним селект

    // Получим список станций
    $.ajax({
        url: '/railway/RWStation/GetStation/',
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

    $.each(newOptions, function (key, value) {
        $('#soflow').append($("", {
            value: key,
            text: value
        }));
    });

    //Первая выборка
    //OnBegin();
    //$.ajax({
    //    url: '/railway/KIST/ListBufferArrivalSostavOfDate/',
    //    type: 'GET',
    //    data: { date_start: d_start.toISOString(), date_stop: d_stop.toISOString(), },
    //    dataType: 'html',
    //    success: function (data) {
    //        selectPeriod(data);
    //    },
    //    error: function (x, y, z) {
    //        LockScreenOff();
    //        alert(x + '\n' + y + '\n' + z);
    //    }
    //});

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
            { "orderData": [2, 1] },
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
        //jQueryUI: false,
    });
    // Отсортируем по времени по убыванию
    table.order([1, 'desc'])
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


