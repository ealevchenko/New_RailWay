//------------------------------------------------------------------------
// Поиск элемента массива по ключу
function getObjects(obj, key, val) {
    var objects = [];
    for (var i in obj) {
        if (!obj.hasOwnProperty(i)) continue;
        if (typeof obj[i] == 'object') {
            objects = objects.concat(getObjects(obj[i], key, val));
        } else if (i == key && obj[key] == val) {
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
            var option = { value: data[i].value, text: data[i].text }
            // Преобразовать формат
            if (typeof callback_option === 'function') {
                option = callback_option(data[i]);
            }

            if (exceptions_value != null) {
                if (exceptions_value.indexOf(option.value) == -1) {
                    options.push("<option value='" + option.value + "' >" + option.text + "</option>");
                }
            } else {
                options.push("<option value='" + option.value + "' >" + option.text + "</option>");
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