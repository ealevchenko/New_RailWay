
//------------------------------------------------------------------------
// Поиск элемента массива по ключу по всем объектам включая и вложенные
function getAllObjects(obj, key, val) {
    var objects = [];
    for (var i in obj) {
        if (!obj.hasOwnProperty(i)) continue;
        if (typeof obj[i] == 'object') {
            objects = objects.concat(getAllObjects(obj[i], key, val));
        } else if (i == key && obj[key] == val) {
            objects.push(obj);
        }
    }
    return objects;
}
// Поиск элемента массива по ключу по первому уровню 
function getObjects(obj, key, val) {
    var objects = [];
    for (var i in obj) {
        if (!obj.hasOwnProperty(i)) continue;
        if (typeof obj[i] == 'object') {
            objects = objects.concat(getChildObjects(obj[i], key, val));
        } else
            if (i == key && obj[key] == val) {
            objects.push(obj);
        }
    }
    return objects;
}
// Поиск элемента массива во вложенных обектах второго уровня 
function getChildObjects(obj, key, val) {
    var objects = [];
    for (var i in obj) {
        if (!obj.hasOwnProperty(i)) continue;
        if (typeof obj[i] == 'object' & false == true) {
            objects = objects.concat(getObjects(obj[i], key, val));
        } else
            if (i == key && obj[key] == val) {
            objects.push(obj);
        }
    }
    return objects;
}


// Получить значение атрибута text по атрибуту value
function getTextOption(obj, val) {
    if (obj != null) {
        for (var i = 0; i < obj.length; i++) {
            if (obj[i].value == val) return obj[i].text;
        }
    }
    return val;
}
//------------------------------------------------------------------------
// Определение параметров переданных по url
$.extend({
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },
    getUrlVar: function (name) {
        return $.getUrlVars()[name];
    }
});
//-------------------------------------------------------------------------
//
function OnBegin() {
    var lang = $.cookie('lang');
    LockScreen(lang == 'en' ? 'We are processing your request ...' : 'Мы обрабатываем ваш запрос...');
}
//-------------------------------------------------------------------------
// Обработчики ajax - функций
// Событие перед запросом
function AJAXBeforeSend() {
    OnBegin();
}
// Обработка ошибок
function OnAJAXError(x, y, z) {
    LockScreenOff();
    if (x.status != 404) {
        alert(x + '\n' + y + '\n' + z);
    }
}
// Событие после выполнения
function AJAXComplete() {
    LockScreenOff();
}

//-------------------------------------------------------------------------
// Инициализация компонента Select
function initSelect(obj_select, property, data, callback_option, value_select, event_change, exceptions_value) {
    var options = [];
    var lang = $.cookie('lang');
    // Проверка выбор неопределен
    if (value_select == -1) {
        options.push("<option value='-1' >" + (lang == 'en' ? 'Select...' : 'Выберите...') + "</option>");
    }
    if (data != null) {
        for (i = 0; i < data.length; i++) {
            var option = { value: data[i].value, text: data[i].text, disabled:data[i].disabled  }
            // Преобразовать формат
            if (typeof callback_option === 'function') {
                option = callback_option(data[i]);
            }
            if (option != null) {
                if (exceptions_value != null) {
                    if (exceptions_value.indexOf(option.value) == -1) {
                        options.push("<option value='" + option.value + "' " + (option.disabled ? "disabled='disabled'" : "") + ">" + option.text + "</option>");
                    }
                } else {
                    options.push("<option value='" + option.value + "' " + (option.disabled ? "disabled='disabled'" : "") + ">" + option.text + "</option>");
                }
            }
        }
    }
    obj_select.empty();
    obj_select.selectmenu({
        icons: { button: "ui-icon ui-icon-circle-triangle-s" },
        width: property.width,
        change: event_change,
    });
    // Заполним селект 
    obj_select.append(options.join(""))
        .val(value_select)
        .selectmenu("refresh");
}
// Инициализация компонента DateTime
function initDateTime(obj_select, property) {
    var lang = $.cookie('lang');
    obj_select.dateRangePicker(
        {
            startOfWeek: 'monday',
            separator: lang == 'en' ? 'to' : 'по',
            language: lang,
            format: lang == 'en' ? 'MM/DD/YYYY HH:mm' : 'DD.MM.YYYY HH:mm',
            autoClose: true,
            singleDate: true,
            showShortcuts: false,
            //getValue: function () {
            //    if ($('#date-start').val() && $('#date-stop').val())
            //        return $('#date-start').val() + ' to ' + $('#date-stop').val();
            //    else
            //        return '';
            //},
            //setValue: function (s, s1, s2) {
            //    $('#date-start').val(s1);
            //    $('#date-stop').val(s2);
            //},
            time: {
                enabled: true
            },
            swapTime: true,
        }).bind('datepicker-closed', function () {

        });
    // Задать дату 
    var dt = new Date();
    var s_dt = dt.getDate() + '.' + (dt.getMonth() + 1) + '.' + dt.getFullYear() + ' ' + dt.getHours() + ':' + dt.getMinutes();
    obj_select.data('dateRangePicker').setDateRange(s_dt, s_dt);
}

//
function initLang(file, callback) {
    $.getJSON(file, function (json) {
        if (typeof callback === 'function') {
            callback(json);
        }
    });
    //.done(function ()
    //{ console.log("second success"); })
    //.fail(function (x, y, z)
    //{ console.log("error"); })
    //.always(function ()
    //{ console.log("complete"); });
}

//-------------------------------------------------------------------------
// Экспорт отчетов в Excel
function fnExcelReport(tab, name_file) {
    var file_name = name_file + '.xls';
    var tab_text = '<html xmlns:x="urn:schemas-microsoft-com:office:excel">';
    tab_text = tab_text + '<head><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet>';

    tab_text = tab_text + '<x:Name>Test Sheet</x:Name>';

    tab_text = tab_text + '<x:WorksheetOptions><x:Panes></x:Panes></x:WorksheetOptions></x:ExcelWorksheet>';
    tab_text = tab_text + '</x:ExcelWorksheets></x:ExcelWorkbook></xml></head><body>';

    tab_text = tab_text + "<table border='1px'>";
    //var tab = $('#table-list-wagons-tracking').html();
    tab_text = tab_text + tab
    tab_text = tab_text + '</table></body></html>';

    var data_type = 'data:application/vnd.ms-excel';

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
        if (window.navigator.msSaveBlob) {
            var blob = new Blob([tab_text], {
                type: "application/csv;charset=utf-8;"
            });
            navigator.msSaveBlob(blob, file_name);
        }
    } else {
        $('#test').attr('href', data_type + ', ' + encodeURIComponent(tab_text));
        $('#test').attr('download', file_name);
    }
}


