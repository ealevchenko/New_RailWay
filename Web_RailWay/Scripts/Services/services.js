$(function () {
    var lang = $.cookie('lang');

    var dateRange,
        date_start, date_stop, 
        id_services_select = null,
        table_status_services


    var dt = new Date();
    var date_start = new Date(dt.getFullYear(), dt.getMonth(), (dt.getDate() - 1));
    var date_stop = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 23, 59, 59);
    var s_d_start = date_start.getDate() + '.' + (date_start.getMonth() + 1) + '.' + date_start.getFullYear() + ' ' + date_start.getHours() + ':' + date_start.getMinutes();
    var s_d_stop = date_stop.getDate() + '.' + (date_stop.getMonth() + 1) + '.' + date_stop.getFullYear() + ' ' + date_stop.getHours() + ':' + date_stop.getMinutes()

    function OnBegin() {
        lang = $.cookie('lang');
        LockScreen(lang == 'en' ? 'We are processing your request ...' : 'Мы обрабатываем ваш запрос...');
    }

    function ViewStatusServices() {
        OnBegin();
        getTbodyStatusServices($('tbody[name ="list-status-services"]'), id_services_select)
        LockScreenOff();
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
    });
    // Установим дату
    dateRange.data('dateRangePicker').setDateRange(s_d_start, s_d_stop, true);
    // Установим картинку
    $('header').first().attr('style', 'background-image: url(.../../../Image/Statistic.jpg)')

    table_status_services = $('#services-table').DataTable({
        "paging": false,
        "ordering": true,
        "info": false,
        language: {
            decimal: lang == 'en' ? "." : ",",
            search: lang == 'en' ? "Search" : "Найти",
        },
        jQueryUI: true,
    });
    table_status_services.draw();

    // Показать статус сервисов
    ViewStatusServices();
});