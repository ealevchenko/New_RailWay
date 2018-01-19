$(document).ready(function () {
    var lang = $.cookie('lang');

    var dateRange,
        date_start, date_stop,
        service_select = null,
        table_status_services, table_log_services, table_log_errors, table_log_events, table_log_setup,
        tabs_logs,
        tabs_active = 0;


    var dt = new Date();
    var date_start = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), (dt.getHours()-2),00,00);
    var date_stop = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 23, 59, 59);
    var s_d_start = date_start.getDate() + '.' + (date_start.getMonth() + 1) + '.' + date_start.getFullYear() + ' ' + date_start.getHours() + ':' + date_start.getMinutes();
    var s_d_stop = date_stop.getDate() + '.' + (date_stop.getMonth() + 1) + '.' + date_stop.getFullYear() + ' ' + date_stop.getHours() + ':' + date_stop.getMinutes()

    function viewStatusServices() {
        // Настроить таблицу
        table_status_services = $('#services-table').DataTable({
            "paging": false,
            "ordering": true,
            "info": false,
            "destroy": true,
            "select": true,
            language: {
                decimal: lang == 'en' ? "." : ",",
                search: lang == 'en' ? "Search" : "Найти",
            },
            jQueryUI: true,
            data: getStatusServices(true),
            "createdRow": function (row, data, index) {
                $(row).attr('service', data.service);
                //$(row).attr('name', 'services-status');
                //$('td', row).eq(0).text(getServicesName(data.service))
                //if (data[5].replace(/[\$,]/g, '') * 1 > 150000) {
                //    $('td', row).eq(5).addClass('highlight');
                //}
            },
            columns: [
                //{ data: "id" },
                { data: "servicename", "class": "details-control" },
                { data: "start" },
                { data: "execution" },
                { data: "current" },
                { data: "max" },
                { data: "min" }
            ]
        });

        $('#services-table tbody').on('click', 'tr', function () {
            //evt.preventDefault();
            $('#services-table tbody tr').removeClass('selected');
            $(this).addClass('selected');
            service_select = $(this).attr("service");
            ViewLog(tabs_active);
        });
    }

    function ViewLog(tabs_active) {

        if (tabs_active == 0) {
            table_log_services = $('#table-log-services').DataTable({
                "paging": true,
                "ordering": true,
                "info": false,
                "destroy": true,
                "select": true,
                "order": [[ 1, 'desc' ]],
                language: {
                    decimal: lang == 'en' ? "." : ",",
                    search: lang == 'en' ? "Search" : "Найти",
                },
                jQueryUI: true,
                data: getLogServices(true, service_select, date_start, date_stop),
                "createdRow": function (row, data, index) {
                    $(row).attr('id', data.id);
                    //$(row).attr('name', 'services-status');
                    //$('td', row).eq(0).text(getServicesName(data.service))
                    //if (data[5].replace(/[\$,]/g, '') * 1 > 150000) {
                    //    $('td', row).eq(5).addClass('highlight');
                    //}
                },
                columns: [
                    //{ data: "id" },
                    { data: "servicename", "class": "details-control" },
                    { data: "start" },
                    { data: "duration" },
                    { data: "code_return" },
                ],
                initComplete: function () {
                    this.api().columns([0]).every(function () {
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
        }
        if (tabs_active == 1) {
            table_log_errors = $('#table-log-errors').DataTable({
                "paging": true,
                "ordering": true,
                "info": false,
                "destroy": true,
                "select": true,
                "order": [[0, 'desc']],
                language: {
                    decimal: lang == 'en' ? "." : ",",
                    search: lang == 'en' ? "Search" : "Найти",
                },
                jQueryUI: true,
                data: getLogErrors(true, service_select, date_start, date_stop),
                "createdRow": function (row, data, index) {
                    $(row).attr('id', data.ID);
                },
                columns: [
                    { data: "DateTime" },
                    //{ data: "UserName" },
                    //{ data: "UserHostName" },
                    //{ data: "UserHostAddress" },
                    //{ data: "PhysicalPath" },
                    { data: "UserMessage" },
                    { data: "servicename" },
                    { data: "eventidname" },
                    { data: "HResult" },
                    { data: "InnerException" },
                    { data: "Message" },
                    //{ data: "Source" },
                    //{ data: "StackTrace" },
                ],
                initComplete: function () {
                    this.api().columns([2, 3]).every(function () {
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
        }
        if(tabs_active == 2){
            table_log_events = $('#table-log-events').DataTable({
                "paging": true,
                //"processing": true,
                "ordering": true,
                "info": false,
                "destroy": true,
                "select": true,
                "order": [[0, 'desc']],
                language: {
                    decimal: lang == 'en' ? "." : ",",
                    search: lang == 'en' ? "Search" : "Найти",
                },
                jQueryUI: true,
                data: getLogEvent(true, service_select, date_start, date_stop),
                "createdRow": function (row, data, index) {
                    $(row).attr('id', data.ID);
                },
                columns: [
                    { data: "DateTime" },
                    //{ data: "UserName" },
                    //{ data: "UserHostName" },
                    //{ data: "UserHostAddress" },
                    //{ data: "PhysicalPath" },
                    { data: "servicename" },
                    { data: "eventidname" },
                    { data: "Event" },
                    { data: "Status" },
                ],
                initComplete: function () {
                    this.api().columns([1, 2]).every(function () {
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
        }
        if (tabs_active == 3) {
            table_log_setup = $('#table-setup').DataTable({
                "paging": true,
                //"processing": true,
                "ordering": true,
                "info": false,
                "destroy": true,
                "select": true,
                "order": [[0, 'desc']],
                language: {
                    decimal: lang == 'en' ? "." : ",",
                    search: lang == 'en' ? "Search" : "Найти",
                },
                jQueryUI: true,
                data: getSetting(true, service_select),
                "createdRow": function (row, data, index) {
                    $(row).attr('id', data.ID);
                },
                columns: [
                    { data: "servicename" },
                    { data: "Description" },
                    { data: "Key" },
                    { data: "Value" },
                    { data: "typevalue" },
                ],
                initComplete: function () {
                    this.api().columns([0]).every(function () {
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
        }
    }

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
        //ViewStatusServices();
    });
    // Установим дату
    dateRange.data('dateRangePicker').setDateRange(s_d_start, s_d_stop, true);
    // Установим картинку
    //$('header').first().attr('style', 'background-image: url(.../../../Image/Statistic.jpg)')
    $('header').first().attr('style', 'background-color:#A2AEC7;')

    tabs = $("#tabs-log").tabs({
        activate: function (event, ui) {
            tabs_active = tabs.tabs("option", "active");
            ViewLog(tabs_active);
        }
    });

    // Показать статус сервисов
    viewStatusServices();
    ViewLog(tabs_active);
});

function OnBegin() {
    var lang = $.cookie('lang');
    LockScreen(lang == 'en' ? 'We are processing your request ...' : 'Мы обрабатываем ваш запрос...');
}
