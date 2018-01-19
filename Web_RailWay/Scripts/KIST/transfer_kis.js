$(document).ready(function () {
    var lang = $.cookie('lang');

    var dateRange,
        date_start, date_stop,
        arrival_select_id = null, input_select_id = null, output_select_id = null,
        table_arrival, table_input, table_output,
        table_detali_cars,
        data_arrival, data_input,
        //table_status_services, table_log_services, table_log_errors, table_log_events, table_log_setup,
        //tabs_logs,
        delete_confirm,
        tabs_active = 0,
        list_numvag_stan = null,
        list_kometa_stan = null,
        list_status = null,
        list_status_name = null;



    var dt = new Date();
    var date_start = new Date(dt.getFullYear(), dt.getMonth(), (dt.getDate()-1));
    var date_stop = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 23, 59, 59);
    var s_d_start = date_start.getDate() + '.' + (date_start.getMonth() + 1) + '.' + date_start.getFullYear() + ' ' + date_start.getHours() + ':' + date_start.getMinutes();
    var s_d_stop = date_stop.getDate() + '.' + (date_stop.getMonth() + 1) + '.' + date_stop.getFullYear() + ' ' + date_stop.getHours() + ':' + date_stop.getMinutes();

    // загрузить библиотеки
    getAsyncNumVagStationName(null, function (result) { list_numvag_stan = result; });
    getAsyncKometaStationName(null, function (result) { list_kometa_stan = result; });
    getAsyncStatus(null, function (result) { list_status = result; });
    getAsyncStatusName(null, function (result) { list_status_name = result; });

    function viewTableBufferArrivalSostav(data) {
            table_arrival = $('#table-arrival').DataTable({
                "paging": false,
                "ordering": true,
                "info": false,
                "destroy": true,
                "select": true,
                "order": [[1, 'desc']],
                language: {
                    decimal: lang == 'en' ? "." : ",",
                    search: lang == 'en' ? "Search" : "Найти",
                },
                jQueryUI: true,
                data: data,
                "createdRow": function (row, data, index) {
                    $(row).attr('id', data.id).addClass(data.status);
                    if (data.id == arrival_select_id) {
                        $(row).addClass('selected');
                        var bac = getObjects(data_arrival, 'id', arrival_select_id)
                        var list_cars_status = getBufferArrivalCarsStatus(bac[0]);
                        viewDetaliCars(tabs_active, list_cars_status)
                    }
                    $('td', row).eq(0).html('<a id='+data.id+' name="natur" href="#">'+data.natur+'</a>')
                    if (data.count_wagons != null) { $('td', row).eq(5).text(data.count_wagons + ' / ' + data.count_set_wagons) };
                    if (data.count_nathist != null) { $('td', row).eq(6).text(data.count_nathist + ' / ' + data.count_set_nathist) };
                    if (data.close != null) {
                        $('td', row).eq(8).text(data.close + ' user:' + data.close_user);
                    } else {
                        $('td', row).eq(8).html('<button id=' + data.id + ' class="ui-button ui-widget ui-corner-all" name="close"><span class="ui-icon ui-icon-circle-close"></span>' + (lang == 'en' ? 'Сlose' : 'Закрыть') + '</button>');
                    };
                },
                columns: [
                    { data: "natur", },
                    { data: "datetime" },
                    { data: "station_name" },
                    { data: "way_num" },
                    { data: "napr" },
                    { data: "count_wagons" },
                    { data: "count_nathist" },
                    { data: "statusname" },
                    { data: "close" },
                ],
                initComplete: function () {
                    this.api().columns([2,7]).every(function () {
                        var column = this;
                        var name = $(column.header()).attr('title');
                        var select = $('<select><option value="">' + (lang == 'en' ? 'All' : 'Все') + '</option></select>')
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
            });

            $('#table-arrival tbody').on('click', 'tr', function () {
                $('#table-arrival tbody tr').removeClass('selected');
                $(this).addClass('selected');
                arrival_select_id = $(this).attr("id");
                var bac = getObjects(data_arrival, 'id', arrival_select_id)
                var list_cars_status = getBufferArrivalCarsStatus(bac[0]);
                viewDetaliCars(tabs_active, list_cars_status)
            });

            $('#table-arrival button[name ="close"]').on('click', function (evt) {
                evt.preventDefault();
                var id = $(this).attr("id")
                $.ajax({
                    url: '/railway/api/kis/transfer/bas/' + id +'/close',
                    type: 'POST',
                    //data: JSON.stringify(new_station_node),
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {
                        viewTransfer(tabs_active);
                    },
                    error: function (x, y, z) {
                        alert(x + '\n' + y + '\n' + z);
                    }
                });
            });

    }

    function viewTableBufferInputSostav(data) {
        table_input = $('#table-input').DataTable({
            "paging": false,
            "ordering": true,
            "info": false,
            "destroy": true,
            "select": true,
            "order": [[1, 'desc']],
            language: {
                decimal: lang == 'en' ? "." : ",",
                search: lang == 'en' ? "Search" : "Найти",
            },
            jQueryUI: true,
            data: data,
            "createdRow": function (row, data, index) {
                $(row).attr('id', data.id).addClass(data.status);
                $('td', row).eq(0).html('<a id=' + data.id + ' name="doc_num" href="#">' + data.doc_num + '</a>')
                //if (data.count_wagons != null) { $('td', row).eq(5).text(data.count_wagons + ' / ' + data.count_set_wagons) };
                //if (data.count_nathist != null) { $('td', row).eq(6).text(data.count_nathist + ' / ' + data.count_set_nathist) };
                if (data.close != null) { $('td', row).eq(10).text(data.close + ' user:' + data.close_user) };
            },
            columns: [
                { data: "doc_num", },
                { data: "datetime" },
                { data: "station_from_name" },
                { data: "way_num_kis" },
                { data: "napr" },
                { data: "station_on_name" },
                { data: "count_wagons" },
                { data: "count_set_wagons" },
                { data: "natur" },
                { data: "statusname" },
                { data: "close" },
            ],
            initComplete: function () {
                this.api().columns([2, 5, 9]).every(function () {
                    var column = this;
                    var name = $(column.header()).attr('title');
                    var select = $('<select><option value="">' + (lang == 'en' ? 'All' : 'Все') + '</option></select>')
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
        });

        $('#table-input tbody').on('click', 'tr', function () {
            $('#table-input tbody tr').removeClass('selected');
            $(this).addClass('selected');
            input_select_id = $(this).attr("id");
            var bic = getObjects(data_input, 'id', input_select_id)
            //var list_cars_status = getBufferArrivalCarsStatus(bic[0]);
            //viewDetaliCars(tabs_active, list_cars_status)
        });
    }

    function viewTableBufferOutputSostav(data) {
        table_input = $('#table-output').DataTable({
            "paging": false,
            "ordering": true,
            "info": false,
            "destroy": true,
            "select": true,
            "order": [[1, 'desc']],
            language: {
                decimal: lang == 'en' ? "." : ",",
                search: lang == 'en' ? "Search" : "Найти",
            },
            jQueryUI: true,
            data: data,
            "createdRow": function (row, data, index) {
                $(row).attr('id', data.id).addClass(data.status);
                $('td', row).eq(0).html('<a id=' + data.id + ' name="doc_num" href="#">' + data.doc_num + '</a>')
                //if (data.count_wagons != null) { $('td', row).eq(5).text(data.count_wagons + ' / ' + data.count_set_wagons) };
                //if (data.count_nathist != null) { $('td', row).eq(6).text(data.count_nathist + ' / ' + data.count_set_nathist) };
                if (data.close != null) { $('td', row).eq(10).text(data.close + ' user:' + data.close_user) };
            },
            columns: [
                { data: "doc_num", },
                { data: "datetime" },
                { data: "station_on_name" },
                { data: "way_num_kis" },
                { data: "napr" },
                { data: "station_from_name" },
                { data: "count_wagons" },
                { data: "count_set_wagons" },
                { data: "statusname" },
                { data: "close" },
            ],
            initComplete: function () {
                this.api().columns([2, 5, 8]).every(function () {
                    var column = this;
                    var name = $(column.header()).attr('title');
                    var select = $('<select><option value="">' + (lang == 'en' ? 'All' : 'Все') + '</option></select>')
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
        });

        $('#table-output tbody').on('click', 'tr', function () {
            $('#table-output tbody tr').removeClass('selected');
            $(this).addClass('selected');
            output_select_id = $(this).attr("id");
            var boc = getObjects(data_input, 'id', input_select_id)
            //var list_cars_status = getBufferArrivalCarsStatus(boc[0]);
            //viewDetaliCars(tabs_active, list_cars_status)
        });
    }

    function viewTransfer(tabs_active) {
        //OnBegin();
        // Прибытие с УЗ
        if (tabs_active == 0) {
            // Настроить таблицу
            //data_arrival = getBufferArrivalSostav(true, date_start, date_stop);
            getAsyncBufferArrivalSostav(
                date_start,
                date_stop,
                function (result) {
                    data_arrival = getCorrectBufferArrivalSostav(result, list_kometa_stan, list_status, list_status_name)
                    viewTableBufferArrivalSostav(data_arrival);
                });

        }
        // По прибытию
        if (tabs_active == 1) {
            // Настроить таблицу
            getAsyncBufferInputSostav(
                date_start,
                date_stop,
                function (result) {
                    var data = getCorrectBufferInputSostav(result,list_numvag_stan,list_status,list_status_name)
                    viewTableBufferInputSostav(data);
                });
        }
        // По отправке
        if (tabs_active == 2) {
            // Настроить таблицу
            getAsyncBufferOutputSostav(
                date_start,
                date_stop,
                function (result) {
                    var data = getCorrectBufferOutputSostav(result, list_numvag_stan, list_status, list_status_name)
                    viewTableBufferOutputSostav(data);
                });
        }
        //LockScreenOff();
    };

    function viewDetaliCars(tabs_active, list_cars_status) {
        if (tabs_active == 0) {
            // Настроить таблицу
            $('#table-detali-cars').show();
            table_detali_cars = $('#table-detali-cars').DataTable({
                "search": false,
                "paging": false,
                "ordering": false,
                "info": false,
                "destroy": true,
                "select": true,
                language: {
                    decimal: lang == 'en' ? "." : ",",
                    search: lang == 'en' ? "Search" : "Найти",
                },
                jQueryUI: true,
                data: list_cars_status,
                "createdRow": function (row, data, index) {
                    //$(row).attr('id', data.id).addClass(data.status);
                    $('td', row).eq(0).html('<a id=' + data.car + ' name="natur" href="#">' + data.car + '</a>')
                    $('td', row).eq(1).addClass(data.car_set);
                    $('td', row).eq(2).addClass(data.car_upderr=='Ok' ? data.car_upderr : 'Error');
                },
                columns: [
                    { data: "car", },
                    { data: "car_set" },
                    { data: "car_upderr" },
                ],
            });
        }
    }

    delete_confirm = $('#delete-confirm').dialog({
        resizable: false,
        modal: true,
        autoOpen: false,
        height: "auto",
        width: 500,
        //buttons: {
        //    "Удалить": Delete,
        //    Cancel: function () {
        //        delete_confirm.dialog("close");
        //    }
        //},
        //close: function () {
        //    form_delete[0].reset();
        //}
    });

    // Настроим компонент период даты
    dateRange = $('#select-range').dateRangePicker(
        {
            startOfWeek: 'monday',
            separator: lang == 'en' ? 'to' : 'по',
            language: lang,
            format: lang == 'en' ? 'MM/DD/YYYY HH:mm' : 'DD.MM.YYYY HH:mm',
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
        }).
        bind('datepicker-change', function (evt, obj) {
            date_start = obj.date1;
            date_stop = obj.date2;
        })
        .bind('datepicker-closed', function () {
            // сделаем действие
            $('#table-detali-cars').hide(); // спрятать таблицу детали
            // Убрать выбранные строки
            arrival_select_id = null;
            input_select_id = null;
            output_select_id = null;
            viewTransfer(tabs_active);
        });
    // Установим дату
    dateRange.data('dateRangePicker').setDateRange(s_d_start, s_d_stop, true);
    // Установим картинку фона
    $('header').first().attr('style', 'background-color:#A2AEC7;')

    tabs = $("#tabs-log").tabs({
        activate: function (event, ui) {
            tabs_active = tabs.tabs("option", "active");
            $('#table-detali-cars').hide(); // спрятать таблицу детали
            viewTransfer(tabs_active);
        }
    });

    // Показать статус сервисов
    viewTransfer(tabs_active);
});

function OnBegin() {
    var lang = $.cookie('lang');
    LockScreen(lang == 'en' ? 'We are processing your request ...' : 'Мы обрабатываем ваш запрос...');
}
