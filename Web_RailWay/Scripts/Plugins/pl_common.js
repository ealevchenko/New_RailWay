
// Список общесистемных слов 
$.Text_Common =
    {
        'default':  //default language: ru
        {
            'mess_delay': 'Мы обрабатываем ваш запрос...',
        },
        'en':  //default language: English
        {
            'mess_delay': 'We are processing your request ...',
        }

    };

/* ----------------- DataTables -------------------------------------------*/
// Список слов для описания таблиц
$.Text_Table =
    {
        'default':  //default language: ru
        {
            "dt_decimal": "",
            "dt_emptyTable": "Нет данных в таблице",
            "dt_info": "Отображение _START_ по _END_ из _TOTAL_ записей",
            "dt_infoEmpty": "Отображение 0 to 0 of 0 записей",
            "dt_infoFiltered": "(отфильтровано из _MAX_ всего записей)",
            "dt_infoPostFix": "",
            "dt_thousands": ".",
            "dt_lengthMenu": "Показать  _MENU_ записей",
            "dt_loadingRecords": "Загрузка...",
            "dt_processing": "Обработка ...",
            "dt_search": "Найти:",
            "dt_zeroRecords": "Не найдено совпадающих записей",
            "dt_paginate": {
                "first": "Первая",
                "last": "Последняя",
                "next": "Следующая",
                "previous": "Предыдущая"
            },
            "dt_aria": {
                "sortAscending": ": активировать сортировку столбца по возрастанию",
                "sortDescending": ": активировать сортировку колонки по убыванию"
            }

        },
        'en':  //default language: English
        {
            "dt_decimal": "",
            "dt_emptyTable": "No data available in table",
            "dt_info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "dt_infoEmpty": "Showing 0 to 0 of 0 entries",
            "dt_infoFiltered": "(filtered from _MAX_ total entries)",
            "dt_infoPostFix": "",
            "dt_thousands": ",",
            "dt_lengthMenu": "Show _MENU_ entries",
            "dt_loadingRecords": "Loading...",
            "dt_processing": "Processing...",
            "dt_search": "Search:",
            "dt_zeroRecords": "No matching records found",
            "dt_paginate": {
                "first": "First",
                "last": "Last",
                "next": "Next",
                "previous": "Previous"
            },
            "dt_aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            }

        }

    };
// Настройка language(DataTables)
var language_table = function (langs) {
    return {
        "decimal": lang('dt_decimal', langs),
        "emptyTable": lang('dt_emptyTable', langs),
        "info": lang('dt_info', langs),
        "infoEmpty": lang('dt_infoEmpty', langs),
        "infoFiltered": lang('dt_infoFiltered', langs),
        "infoPostFix": lang('dt_infoPostFix', langs),
        "thousands": lang('dt_thousands', langs),
        "lengthMenu": lang('dt_lengthMenu', langs),
        "loadingRecords": lang('dt_loadingRecords', langs),
        "processing": lang('dt_processing', langs),
        "search": lang('dt_search', langs),
        "zeroRecords": lang('dt_zeroRecords', langs),
        "paginate": lang('dt_paginate', langs),
        "aria": lang('dt_aria', langs),
    }
}

/* ------------------------------------------------------------------------*/
// Метод определения списка по указаному языку
var getLanguages = function (languages, lang) {
    if (lang == 'auto') {
        var language = navigator.language ? navigator.language : navigator.browserLanguage;
        if (!language) return languages['default'];
        var language = language.toLowerCase();
        for (var key in languages) {
            if (language.indexOf(key) != -1) {
                return languages[key];
            }
        }
        return languages['default'];
    }
    else if (lang && lang in languages) {
        return languages[lang];
    }
    else {
        return languages['default'];
    }
};
// Показать текст
var lang = function (t, langs) {
    var _t = t.toLowerCase();
    var re = (t in langs) ? langs[t] : (_t in langs) ? langs[_t] : null;
    return re;
};

/* ------------------------------------------------------------------------*/
// Показать значение поля форматированный вывод
var fieldText = function (field, data_string) {
    if ((field) && field.value != null) {
        var text = field.value.str;
        for (i = 0; i < field.value.val.length; i++) {
            text = text.replace(new RegExp('#' + i, 'g'), data_string[field.value.val[i]]);
        }
        return text;
    }
}

/* ------------------------------------------------------------------------*/
var getID = function (obj) {
    return obj.attr("id");
};
// Показать сообщение
var messageDelay = function (message) {
    LockScreen(message);
};


