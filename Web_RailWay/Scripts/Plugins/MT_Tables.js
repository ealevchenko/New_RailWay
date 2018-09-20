(function ($) {


    // Список общесистемных слов 
    $.MTCommon =
    {
        'default':  //default language: ru
        {
            'mess_delay': 'Мы обрабатываем ваш запрос...',
            'label_view_first': 'Прибытие',
            'label_view_last': 'Отправка',
        },
        'en':  //default language: English
        {
            'mess_delay': 'We are processing your request ...',
            'label_view_first': 'Arrival',
            'label_view_last': 'Sending',
        }

    };

    // Список слов для описания таблиц
    $.MTTable =
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

    // Список слов
    $.MTFields =
    {
        'default':  //default language: ru
        {
            'field_id_sostav': 'IDSostav',
            'field_num': '№ вагона',
            'field_composition_index': 'Индекс поезда',
            'field_date_operation': 'Дата и время операции',
            'field_position': 'Позиция',
            'field_operation': 'Операция',
            'field_consignee': 'Грузополучатель',
            'field_cargo': 'Груз',
            'field_station_end': 'Станция назначения',
            'field_weight_cargo': 'Вес груза',
            'field_country': 'Страна',
            'field_train_number': '№ поезда',
            'field_arrival_car': 'Вагон принят',
            'field_arrival_doc': 'Документ принятия',
            'field_user_name': 'Имя пользователя',
        },
        'en':  //default language: English
        {
            'field_id_sostav': 'IDSostav',
            'field_num': '# Car',
            'field_composition_index': 'Train index',
            'field_date_operation': 'Operation date and time',
            'field_position': 'Position',
            'field_operation': 'Operation',
            'field_consignee': 'Consignee',
            'field_cargo': 'Cargo',
            'field_station_end': 'Destination station',
            'field_weight_cargo': 'Cargo weight',
            'field_country': 'Country',
            'field_train_number': 'Train number',
            'field_arrival_car': 'Car accepted',
            'field_arrival_doc': 'Acceptance document',
            'field_user_name': 'Username',

        }

    };
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
        //var defaultLanguage = $.MTLanguages['default'];
        //if (re == null) re = (t in defaultLanguage) ? defaultLanguage[t] : (_t in defaultLanguage) ? defaultLanguage[_t] : '';
        return re;
    };

    var getID = function (obj) {
        return obj.attr("id");
    };
    // Показать сообщение
    var messageDelay = function (message) {
        LockScreen(message);
    };

    // Плагин отобразить вагон по прибытию
    $.fn.mtArrivalCar = function (method) {

        var name_plagin = "mtArrivalCar";
        //var data = null;
        var defaults = {
            language: 'auto',                   // локализация
            message_delay: true,                // Показывать сообщение о задержке
            // таблица
            class_table: "Display compact",     // класс таблицы
            paging: true,
            ordering: true,
            info: false,
            scrollY: null,
            scrollX: true,

            default_sort_arrival: true,         // показать отсортированые по id прибытию true - по убыванию, false - по умолчанию

            detali: false,
            detali_first: true,                 // отображать пребытие на станцию - true или отпраку со станции -false

            num_car: null,                      // показать информацию по указаному вагону
            list_cars: null,                    // показать информацию по указаному вагону в указаной таблице

            // Работа с библиотеками
            reference_result: null,             // массив Справочник результатов
            reference_states: null,             // массив Справочник стран СНГ
        };

        //var reference_result = null; // массив Справочник результатов
        //var reference_states = null; // массив Справочник стран СНГ

        var methods = {
            init: function (params) {
                return this.each(function (i, el) {
                    var $this = $(el),
                    //data = $this.data(name_plagin + getID($this));
                    data = $this.data();
                    // Если плагин ещё не проинициализирован
                    if (!data.id) {
                        var table_name = 'table-' + getID($this);
                        $(this).data('id', table_name);
                        var option;
                        // Выполним инициализацию
                        if (!params) option = {};
                        option = $.extend(true,
                            defaults,
                            params,
                            {
                                id_table: table_name,

                            }
                            );
                        $(this).data('option', option);

                        // Справочник лаколизации
                        //langs = getLanguages($.MTLanguages, option.language);

                        var langs = $.extend(true, $.extend(true, getLanguages($.MTFields, option.language), getLanguages($.MTCommon, option.language)), getLanguages($.MTTable, option.language));
                        $(this).data('langs', langs);

                        $this.append($('<table class="' + option.class_table + '" id="' + table_name + '" cellpadding="0" style="width:100%"></table>'))

                        var obj = initTableListOperation(table_name, option, langs, 0);
                        $(this).data('object', obj);

                        // Если указан вагон показать все вагоны
                        if (option.num_car != null) {
                            methods.viewCar.call($this, option.num_car);
                        }
                        if (option.list_cars != null) {
                            methods.viewCars.call($this, option.list_cars);
                        }
                        initPanel($this.data());
                        initEventSelectChild($this.data());

                    }
                });
            },
            // Показать вагон
            viewCar: function (num) {
                return this.each(function (i, el) {
                    var $this = $(el);
                    var data = $this.data()
                    loadData(data, num,
                        function (result) {
                            data.history = result;
                            viewRows(data);
                            //$this.data('option', data.option);
                        });
                });


                return $this;
            },
            // Показать вагон
            viewCarOfData: function (history) {
                return this.each(function (i, el) {
                    var $this = $(el);
                    var data = $this.data()
                    data.history = history;
                    viewRows(data);
                    //$this.data('option', data.option);
                });
                return $this;
            },
            // Показать вагоны в разных таблицах
            viewCars: function (list_cars) {
                return this.each(function (i, el) {
                    var $this = $(el);
                    var data = $this.data()
                    var num = lang(data.id, list_cars)
                    loadData(data, num,
                        function (result) {
                            data.history = result;
                            viewRows(data);
                            //$this.data('option', data.option);
                        });

                });
                return $this;
            },


            destroy: function () {
                return this.each(function () {

                    var $this = $(this),
                        data = $this.data(name_plagin + getID($this));

                    $(window).unbind('.' + name_plagin + getID($this));
                    data.tooltip.remove();
                    $this.removeData(name_plagin + getID($this));
                })
            },
        }

        // немного магии
        if (methods[method]) {
            // если запрашиваемый метод существует, мы его вызываем
            // все параметры, кроме имени метода прийдут в метод
            // this так же перекочует в метод
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            // если первым параметром идет объект, либо совсем пусто
            // выполняем метод init
            return methods.init.apply(this, arguments);
        } else {
            // если ничего не получилось
            $.error('Метод "' + method + '" не найден в плагине jQuery.' + name_plagin);
        };

        // Инициализация таблицы
        function initTableListOperation(id, option, langs) {
            return $('#' + id).DataTable({
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                "paging": option.paging,
                "ordering": option.ordering,
                "info": option.info,
                //"select": false,
                //"filter": true,
                "scrollY": option.scrollY,
                "scrollX": option.scrollX,
                language: {
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
                },
                jQueryUI: true,
                "createdRow": function (row, data, index) {
                    //$(row).attr('id', type == 0 ? data.id_history : data.ID);
                },
                columns: [
                    {
                        className: 'details-control',
                        orderable: false,
                        data: null,
                        defaultContent: '',
                        searchable: false,
                        width: "30px",
                        visible: option.detali
                    },
                    { data: "IDSostav", title: lang('field_id_sostav', langs), width: "50px", orderable: false, searchable: true },
                    { data: "Num", title: lang('field_num', langs), width: "50px", orderable: false, searchable: false },
                    { data: "CompositionIndex", title: lang('field_composition_index', langs), width: "100px", orderable: false, searchable: false },
                    { data: "DateOperation", title: lang('field_date_operation', langs), width: "100px", orderable: true, searchable: false },
                    { data: "Position", title: lang('field_position', langs), width: "50px", orderable: false, searchable: false },
                    { data: "Operation", title: lang('field_operation', langs), width: "50px", orderable: false, searchable: false },
                    { data: "Consignee", title: lang('field_consignee', langs), width: "50px", orderable: false, searchable: false },
                    { data: "Cargo", title: lang('field_cargo', langs), width: "200px", orderable: false, searchable: false },
                    { data: "StationEnd", title: lang('field_station_end', langs), width: "150px", orderable: false, searchable: false },
                    { data: "Weight", title: lang('field_weight_cargo', langs), width: "50px", orderable: false, searchable: false },
                    { data: "Country", title: lang('field_country', langs), width: "50px", orderable: false, searchable: false },
                    { data: "TrainNumber", title: lang('field_train_number', langs), width: "50px", orderable: false, searchable: false },
                    { data: "Arrival", title: lang('field_arrival_car', langs), width: "150px", orderable: false, searchable: false },
                    { data: "NumDocArrival", title: lang('field_arrival_doc', langs), width: "150px", orderable: false, searchable: false },
                    { data: "UserName", title: lang('field_user_name', langs), width: "150px", orderable: false, searchable: false },
                ],
            });
        };

        // Инициализация события детально
        function initEventSelectChild(data) {
            if (!data.option.detali) return;
            $('#'+data.id).find('tbody')
                .on('click', 'td.details-control', function () {
                    var tr = $(this).closest('tr');
                    var row = data.object.row(tr);
                    if (row.child.isShown()) {
                        // This row is already open - close it
                        row.child.hide();
                        tr.removeClass('shown');
                    }
                    else {
                        row.child('<div id="detali-arrival-operation-' + row.data().id_history + '" class="detali-operation">' +
                            '<table class="table-cars cell-border" id="table-detali-arrival-operation-' + row.data().id_history + '" style="width:100%" cellpadding="0"></table>' +
                            '</div>').show();
                        // Инициализируем
                        viewTableChildOperation(row.data(), data);
                        tr.addClass('shown');
                    }
                });
        };
        // Показать таблицу детали операции
        function viewTableChildOperation(row_data, data) {

            if ($.fn.dataTable.isDataTable('#table-detali-arrival-operation-' + row_data.id_history)) {
                detali_operation = $('#table-detali-arrival-operation-' + row_data.id_history).DataTable();
            }
            else {
                detali_operation = $('#table-detali-arrival-operation-' + row_data.id_history).mtArrivalCar({
                    paging: false,
                    ordering: false,
                    info: false,
                    scrollY: null,
                    scrollX: false,
                    reference_result: data.option.reference_result,
                    reference_states: data.option.reference_states
                });

                // Обновим данные
                var history_arrival = getObjects(data.history, 'id', row_data.id_history);
                if (history_arrival != null && history_arrival.length > 0) {
                    detali_operation.mtArrivalCar("viewCarOfData", history_arrival[0].history);
                }
            } // end else if
        };

        // Инициализация панели
        function initPanel(data) {
            if (!data.option.detali) return;
            var selectViewOperation = function (e) {
                var target = $(e.target);
                if (target.is("#view-first-operation-" + data.id)) {
                    data.option.detali_first = true;
                }
                if (target.is("#view-last-operation-" + data.id)) {
                    data.option.detali_first = false;
                }
                viewRows(data)
            };

            data.option.html_div_panel = $('<div class="dt-buttons setup-operation" id="property-' + data.id + '"></div>');
            //if (data.option.html_div_panel != null) data.option.html_div_panel.empty();
            data.option.html_div_panel_select = $('<div class="setup-operation" id="view-select-' + data.id + '"></div>');
            //if (data.option.html_div_panel_select != null) data.option.html_div_panel_select.empty();
            // Выбор отображения 
            data.option.label_view_first = $('<label for="view-first-operation-' + data.id + '">'+ lang('label_view_first', data.langs) + '</label>');
            data.option.radio_view_first = $('<input type="radio" name="mode-' + data.id + '" id="view-first-operation-' + data.id + '" ' + (data.option.detali_first ? 'checked="checked"':'') + '>');
            data.option.label_view_last = $('<label for="view-last-operation-' + data.id + '">'+lang('label_view_last', data.langs)+'</label>');
            data.option.radio_view_last = $('<input type="radio" name="mode-' + data.id + '" id="view-last-operation-' + data.id + '" ' + (!data.option.detali_first ? 'checked="checked"' : '') + '>');

            // Настроим панель info
            data.option.html_div_panel_select
                .append(data.option.label_view_first)
                .append(data.option.radio_view_first)
                .append(data.option.label_view_last)
                .append(data.option.radio_view_last)
            data.option.html_div_panel
                //.append(this.html_div_panel_info)
                .append(data.option.html_div_panel_select);
            $('DIV#'+data.id+'_wrapper').prepend(data.option.html_div_panel);
            data.option.html_div_panel_select.controlgroup();
            // определим событие выбора режима
            data.option.radio_view_first.on("change", selectViewOperation);
            data.option.radio_view_last.on("change", selectViewOperation);
        };

        // Получить список строк по указаному вагону
        function loadData(data, num, callback) {
            //выводим сообщение
            if (data.option.message_delay) messageDelay(lang('mess_delay', data.langs));
            // Загружаем библиотеку
            loadReference(data, function (result) {
                // Обновим данные
                getAsyncArrivalCars(
                    num,
                    function (result) {
                        // Скорректировать результат если детально
                        if (data.option.detali) {
                          result = createHistory(result);
                        }
                        if (typeof callback === 'function') {
                            callback(result);
                        }
                    }
                );
            });
        };
        // Добавить строку
        function addRow(data, row, id_history) {
            var country = data.option.reference_states != null ? data.option.reference_states.getCountry(row.CountryCode) : null;
            var result = row.NumDocArrival <= 0 ? data.option.reference_result != null ? data.option.reference_result.getResult(row.NumDocArrival).text + '(' + row.NumDocArrival + ')' : null : row.NumDocArrival;
            data.object.row.add({
                "id_history": id_history,
                "ID": row.ID,
                "IDSostav": row.IDSostav,
                "Position": row.Position,
                "Num": row.Num,
                "Country": country!= null ? country.state + '(' + row.CountryCode + ')' : '?',
                //"Country": row.CountryCode,
                "CountryCode": row.CountryCode,
                "Weight": row.Weight,
                "CargoCode": row.CargoCode,
                //"Cargo": history.Cargo,
                "Cargo": row.Cargo + '(' + row.CargoCode + ')',
                "StationCode": row.StationCode,
                "Station": row.Station,
                "StationEnd": row.Station + '(' + row.StationCode + ')',
                "Consignee": row.Consignee,
                "Operation": row.Operation,
                "CompositionIndex": row.CompositionIndex,
                "DateOperation": row.DateOperation,
                "TrainNumber": row.TrainNumber,
                "NumDocArrival": result,
                "Arrival": row.Arrival,
                "ParentID": row.ParentID,
                "UserName": row.UserName,
            });
        };
        // Добавить строки
        function addRows(data) {
            data.object.clear();
            for (i = 0; i < data.history.length; i++) {
                if (data.option.detali) {
                    addRow(data, data.option.detali_first ? data.history[i].first : data.history[i].last, data.history[i].id);
                } else {
                    addRow(data, data.history[i], null);
                }

                
            }
        };
        // Добавить и показать строки
        function viewRows(data) {
            addRows(data);
            data.object.order([1, data.option.default_sort_arrival ? 'desc' : 'asc']); // Отсортировать
            data.object.draw();
            //if (data.option.detali) {
            //    //initPanel(data);
            //    initEventSelectChild(data);
            //}
            // Закрываем сообщения
            if (data.option.message_delay) LockScreenOff();
        };

        // ----------------------------------------------------------------
        // Получить историю по вагону
        function getHistory(obj, list, global_list) {
            list.push(obj);
            var first_arrival = getObjects(global_list, 'ParentID', obj.ID);
            if (first_arrival != null && first_arrival.length > 0) {
                getHistory(first_arrival[0], list, global_list);
            }
        };
        // Сформировать историю по вагону
        function createHistory(data) {
            // Сбросим историю
            var list_history_car = [];
            // Заполним новую
            var first_arrival = getObjects(data, 'ParentID', null);
            if (first_arrival != null && first_arrival.length > 0) {
                for (i = 0; i < first_arrival.length; i++) {
                    var objects = [];
                    getHistory(first_arrival[i], objects, data);
                    list_history_car.push({ id: i + 1, first: first_arrival[i], last: objects[objects.length - 1], history: objects });
                }
            }
            return list_history_car;
        };

        // Загрузка библиотек
        function loadReference(data, callback) {
            var count = 2;
            // Загрузка библиотеки результатов  (metallurgtrans.js)
            if (data.option.reference_result == null) {
                getReferenceArrivalResult(function (result) {
                    data.option.reference_result = result;
                    count -= 1;
                    if (count <= 0) {
                        if (typeof callback === 'function') {
                            callback(data);
                        }
                    }
                })
            } else {
                count -= 1;
            }
            // Загрузка библиотеки Справочник стран СНГ  (reference.js)
            if (data.option.reference_states == null) {
                getReferenceStates(function (result) {
                    data.option.reference_states = result;
                    count -= 1;
                    if (count <= 0) {
                        if (typeof callback === 'function') {
                            callback(data);
                        }
                    }
                })
            } else {
                count -= 1;
            }
            //
            if (count <= 0) {
                if (typeof callback === 'function') {
                    callback(data);
                }
            }
        };
    };

})(jQuery);