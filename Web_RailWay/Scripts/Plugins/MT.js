(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define(['jquery', 'moment'], factory);
    } else if (typeof exports === 'object' && typeof module !== 'undefined') {
        // CommonJS. Register as a module
        module.exports = factory(require('jquery'), require('moment'));
    } else {
        // Browser globals
        factory(jQuery);
    }
}
(function ($, moment) {

    $.MTLanguages =
    {
        'default':  //default language: ru
        {
            'selected': 'Selected:',
            'day': 'Day',
        },
        'en':  //default language: English
        {
            'selected': 'Selected:',
            'day': 'Day',
        }

    };

    var getLanguages = function (lang) {
        if (lang == 'auto') {
            var language = navigator.language ? navigator.language : navigator.browserLanguage;
            if (!language) return $.MTLanguages['default'];
            var language = language.toLowerCase();
            for (var key in $.MTLanguages) {
                if (language.indexOf(key) != -1) {
                    return $.MTLanguages[key];
                }
            }
            return $.MTLanguages['default'];
        }
        else if (opt.language && opt.language in $.MTLanguages) {
            return $.MTLanguages[opt.language];
        }
        else {
            return $.MTLanguages['default'];
        }
    };




    $.fn.mtHistoryArrivalCarDetali = function (opt) {
        if (!opt) opt = {};
        opt = $.extend(true,
		{
		    autoClose: false,
		    format: 'YYYY-MM-DD',
		    language: 'auto',
		}, opt);

        var langs = getLanguages(opt.language);

        var initiated = false;

        init_mtHistoryArrivalCarDetali.call(this);

        //$(window).bind('resize.datepicker', calcPosition);

        return this;

        function init_mtHistoryArrivalCarDetali() {
            var self = this;

            }
            //// Инициализация справочника лаколизации
            //function getLanguages() {
            //    if (opt.language == 'auto') {
            //        var language = navigator.language ? navigator.language : navigator.browserLanguage;
            //        if (!language) return $.MTLanguages['default'];
            //        var language = language.toLowerCase();
            //        for (var key in $.MTLanguages) {
            //            if (language.indexOf(key) != -1) {
            //                return $.MTLanguages[key];
            //            }
            //        }
            //        return $.MTLanguages['default'];
            //    }
            //    else if (opt.language && opt.language in $.MTLanguages) {
            //        return $.MTLanguages[opt.language];
            //    }
            //    else {
            //        return $.MTLanguages['default'];
            //    }
            //}

            /**
             * translate language string
             */
            function lang(t) {
                var _t = t.toLowerCase();
                var re = (t in langs) ? langs[t] : (_t in langs) ? langs[_t] : null;
                var defaultLanguage = $.MTLanguages['default'];
                if (re == null) re = (t in defaultLanguage) ? defaultLanguage[t] : (_t in defaultLanguage) ? defaultLanguage[_t] : '';
                return re;
            }


        };

    })
);

    //(function ($) {

    //    var methods = {
    //        init: function (options) {
    //            return this.each(function () {

    //                var $this = $(this),
    //                    data = $this.data('mtHistoryArrivalCarDetali'),
    //                    tooltip = $('<div />', {
    //                        text: $this.attr('title')
    //                    });

    //                // Если плагин ещё не проинициализирован
    //                if (!data) {

    //                    // Выполним инициализацию
    //                    $this.append($('<table class="compact table-cars cell-border" id="table-list-history-arrival" cellpadding="0" style="width:100%"></table>'))

    //                    // Поставим отметку
    //                    $(this).data('mtHistoryArrivalCarDetali', {
    //                        target: $this,
    //                        tooltip: tooltip
    //                    });

    //                }
    //            });
    //        },
    //        show: function () {

    //        },
    //        hide: function () {

    //        },
    //        update: function (content) {
    //            // !!!
    //        },
    //        destroy: function () {
    //            return this.each(function () {

    //                var $this = $(this),
    //                    data = $this.data('mtHistoryArrivalCarDetali');

    //                // пространства имён рулят!!11
    //                $(window).unbind('.mtHistoryArrivalCarDetali');
    //                data.tooltip.remove();
    //                $this.removeData('mtHistoryArrivalCarDetali');
    //            })
    //        },
    //    };

    //    $.fn.mtHistoryArrivalCarDetali = function (method) {

    //        // Тут пишем функционал нашего плагина
    //        // логика вызова метода
    //        if (methods[method]) {
    //            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
    //        } else if (typeof method === 'object' || !method) {
    //            return methods.init.apply(this, arguments);
    //        } else {
    //            $.error('Метод с именем ' + method + ' не существует для jQuery.mtHistoryArrivalCarDetali');
    //        }

    //    };

    //})(jQuery);

    (function ($) {

        var table_history_arrival = {
            html_table: $('#table-list-history-arrival'),
            html_div_panel: $('<div class="dt-buttons setup-operation" id="property"></div>'),
            html_div_panel_select: $('<div class="setup-operation" id="view-select"></div>'),
            // Выбор отображения 
            label_view_first: $('<label for="view-first-operation"></label>'),
            radio_view_first: $('<input type="radio" name="mode" id="view-first-operation">'),
            label_view_last: $('<label for="view-last-operation"></label>'),
            radio_view_last: $('<input type="radio" name="mode" id="view-last-operation" checked="checked" >'),
            obj_table: null,
            obj: null,
            list: [],
            list_history_car: [],
            type_history: 1,        // отображать пребытие на станцию - 0 или отпраку со станции -1
            type_load: 0,           // загрузить справочники сразу - 0 или запрос производить по id - 1
            reference_states: [],   // справочник список стран
            reference_result: [],   // справочник результатов
            // Инициализация панели
            initPanel: function () {
                // Настроим панель info
                this.html_div_panel_select
                    .append(this.label_view_first.text(resurses.getText("label_view_first")))
                    .append(this.radio_view_first)
                        .append(this.label_view_last.text(resurses.getText("label_view_last")))
                    .append(this.radio_view_last)
                this.html_div_panel
                    //.append(this.html_div_panel_info)
                    .append(this.html_div_panel_select);
                this.obj_table.prepend(this.html_div_panel);
                this.html_div_panel_select.controlgroup();
                // определим событие выбора режима
                this.radio_view_first.on("change", this.selectViewOperation);
                this.radio_view_last.on("change", this.selectViewOperation);
            },
            // Определение события выбора отображения операции
            selectViewOperation: function (e) {
                var target = $(e.target);
                if (target.is("#view-first-operation")) {
                    table_history_arrival.type_history = 0;
                }
                if (target.is("#view-last-operation")) {
                    table_history_arrival.type_history = 1;
                }
                table_history_arrival.viewTable(false);
            },
            // Загрузка библиотек
            initReference: function (callback) {
                getReferenceArrivalResult(function (result) {
                    table_history_arrival.reference_result = result;
                })
                // Справочник стран СНГ
                getReferenceStates(function (result) {
                    table_history_arrival.reference_states = result;
                    if (typeof callback === 'function') {
                        callback();
                    }
                });
            },
            // Инициализировать таблицы операции
            initTableListOperation: function (html_table, type) {
                return $(html_table).DataTable({
                    "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                    "paging": type == 0 ? true : false,
                    "ordering": true,
                    "info": false,
                    //"select": false,
                    //"filter": true,
                    //"scrollY": "600px",
                    "scrollX": true,
                    language: {
                        emptyTable: resurses.getText("table_message_emptyTable"),
                        decimal: resurses.getText("table_decimal"),
                        search: resurses.getText("table_message_search"),
                    },
                    jQueryUI: true,
                    "createdRow": function (row, data, index) {
                        $(row).attr('id', type == 0 ? data.id_history : data.ID);
                    },
                    columns: [
                        {
                            className: 'details-control',
                            orderable: false,
                            data: null,
                            defaultContent: '',
                            searchable: false,
                            width: "30px",
                            visible: type == 0 ? true : false
                        },
                        { data: "IDSostav", title: resurses.getText("table_field_id_sostav"), width: "50px", orderable: false, searchable: true },
                        { data: "Num", title: resurses.getText("table_field_num"), width: "50px", orderable: false, searchable: false },
                        { data: "CompositionIndex", title: resurses.getText("table_field_composition_index"), width: "100px", orderable: false, searchable: false },
                        { data: "DateOperation", title: resurses.getText("table_field_date_operation"), width: "100px", orderable: false, searchable: false },
                        { data: "Position", title: resurses.getText("table_field_position"), width: "50px", orderable: false, searchable: false },
                        { data: "Operation", title: resurses.getText("table_field_operation"), width: "50px", orderable: false, searchable: false },
                        { data: "Consignee", title: resurses.getText("table_field_consignee"), width: "50px", orderable: false, searchable: false },
                        { data: "Cargo", title: resurses.getText("table_field_cargo"), width: "200px", orderable: false, searchable: false },
                        { data: "StationEnd", title: resurses.getText("table_field_station_end"), width: "150px", orderable: false, searchable: false },
                        { data: "Weight", title: resurses.getText("table_field_weight_cargo"), width: "50px", orderable: false, searchable: false },
                        { data: "Country", title: resurses.getText("table_field_country"), width: "50px", orderable: false, searchable: false },
                        { data: "TrainNumber", title: resurses.getText("table_field_train_number"), width: "50px", orderable: false, searchable: false },
                        { data: "Arrival", title: resurses.getText("table_field_arrival_car"), width: "150px", orderable: false, searchable: false },
                        { data: "NumDocArrival", title: resurses.getText("table_field_arrival_doc"), width: "150px", orderable: false, searchable: false },
                        { data: "UserName", title: resurses.getText("table_field_user_name"), width: "150px", orderable: false, searchable: false },
                    ],
                });
            },
            // Инициализировать весь объект
            initObject: function () {
                if (this.type_load == 0) {
                    this.initReference(function () {
                        // После загрузки библиотек инициализируем таблицу
                        table_history_arrival.initObjectTable();
                    });
                } else {
                    table_history_arrival.initObjectTable();
                }

            },
            // Инициализировать таблицу
            initObjectTable: function () {
                this.obj = this.initTableListOperation(this.html_table, 0);
                this.obj_table = $('DIV#table-list-history-arrival_wrapper');
                this.initPanel();
                //this.initEventSelect();
                this.initEventSelectChild();
                this.obj.order([1, 'desc']);
            },
            // Показать таблицу с данными
            viewTable: function (data_refresh) {
                OnBegin();
                if (this.list_history_car == null | data_refresh == true) {
                    // Обновим данные
                    getAsyncArrivalCars(
                        svagon.spinner("value"),
                        function (result) {
                            table_history_arrival.list = result;
                            table_history_arrival.createHistory(result)
                            table_history_arrival.loadDataTable(table_history_arrival.list_history_car);
                            table_history_arrival.obj.draw();
                        }
                    );
                } else {
                    table_history_arrival.loadDataTable(this.list_history_car);
                    table_history_arrival.obj.draw();
                };
            },
            // Получить историю по вагону
            getHistory: function (obj, list, global_list) {
                list.push(obj);
                var first_arrival = getObjects(global_list, 'ParentID', obj.ID);
                if (first_arrival != null && first_arrival.length > 0) {
                    table_history_arrival.getHistory(first_arrival[0], list, global_list);
                }
            },
            // Сформировать историю по вагону
            createHistory: function (data) {
                // Сбросим историю
                table_history_arrival.list_history_car = [];
                // Заполним новую
                var first_arrival = getObjects(data, 'ParentID', null);
                if (first_arrival != null && first_arrival.length > 0) {
                    for (i = 0; i < first_arrival.length; i++) {
                        var objects = [];
                        table_history_arrival.getHistory(first_arrival[i], objects, data);
                        table_history_arrival.list_history_car.push({ id: i + 1, first: first_arrival[i], last: objects[objects.length - 1], history: objects });
                    }
                }
            },
            // Добавить строку операции
            addRow: function (obj, i, history, id_history) {

                if (this.type_load == 0) {
                    // взять данные из внутреннего справочника
                    var country = this.reference_states != null ? this.reference_states.getCountry(history.CountryCode) : null;
                    var result = history.NumDocArrival <= 0 ? this.reference_result != null ? this.reference_result.getResult(history.NumDocArrival).text + '(' + history.NumDocArrival + ')' : null : history.NumDocArrival;
                } else {
                    // получить данные из сервера
                    this.loadFieldCountry(obj, i, history.CountryCode);
                    this.loadFieldResult(obj, i, history.NumDocArrival);
                }
                obj.row.add({
                    "id_history": id_history,
                    "ID": history.ID,
                    "IDSostav": history.IDSostav,
                    "Position": history.Position,
                    "Num": history.Num,
                    "Country": this.type_load == 0 ? country.state + '(' + history.CountryCode + ')' : history.CountryCode,
                    //"Country": history.CountryCode,
                    "CountryCode": history.CountryCode,
                    "Weight": history.Weight,
                    "CargoCode": history.CargoCode,
                    //"Cargo": history.Cargo,
                    "Cargo": history.Cargo + '(' + history.CargoCode + ')',
                    "StationCode": history.StationCode,
                    "Station": history.Station,
                    "StationEnd": history.Station + '(' + history.StationCode + ')',
                    "Consignee": history.Consignee,
                    "Operation": history.Operation,
                    "CompositionIndex": history.CompositionIndex,
                    "DateOperation": history.DateOperation,
                    "TrainNumber": history.TrainNumber,
                    "NumDocArrival": this.type_load == 0 ? result : history.NumDocArrival,
                    "Arrival": history.Arrival,
                    "ParentID": history.ParentID,
                    "UserName": history.UserName,
                });
            },
            // Загрузить данные
            loadDataTable: function (data) {
                this.obj.clear();

                for (i = 0; i < data.length; i++) {
                    if (this.type_history == 0) {
                        var history = data[i].first;
                    };
                    if (this.type_history == 1) {
                        var history = data[i].last;
                    };
                    //var country = thisreference_states != null ? this.reference_states.getCountry(history.CountryCode) : null;
                    this.addRow(this.obj, i, history, data[i].id);
                }
                LockScreenOff();
            },
            // Загрузить поле страна
            loadFieldCountry: function (obj, row, code) {
                getAsyncStatesOfID(code, function (result) {
                    obj.cell(row, 11).data(result.state + '(' + code + ')').draw();
                });
            },
            // Загрузить поле страна
            loadFieldResult: function (obj, row, num) {
                getAsyncArrivalResult(num, function (result) {
                    obj.cell(row, 14).data(result.text + '(' + result.value + ')').draw();
                });
            },
            // Инициализация события детально
            initEventSelectChild: function () {
                this.html_table.find('tbody')
                    .on('click', 'td.details-control', function () {
                        var tr = $(this).closest('tr');
                        var row = table_history_arrival.obj.row(tr);
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
                            table_history_arrival.viewTableChildOperation(row.data());
                            tr.addClass('shown');
                        }
                    });
            },
            // Показать таблицу детали операции
            viewTableChildOperation: function (data) {

                if ($.fn.dataTable.isDataTable('#table-detali-arrival-operation-' + data.id_history)) {
                    detali_operation = $('#table-detali-arrival-operation-' + data.id_history).DataTable();
                }
                else {
                    detali_operation = this.initTableListOperation('#table-detali-arrival-operation-' + data.id_history, 1);
                    // Обновим данные
                    var history_arrival = getObjects(this.list_history_car, 'id', data.id_history);
                    if (history_arrival != null && history_arrival.length > 0) {
                        detali_operation.clear();
                        for (i = 0; i < history_arrival[0].history.length; i++) {
                            var history = history_arrival[0].history[i];
                            this.addRow(detali_operation, i, history, null);
                        }
                    }
                } // end else if
                detali_operation.draw();
            },
        }

        var methods = {
            init: function (options) {
                return this.each(function () {

                    var $this = $(this),
                        data = $this.data('mt_arrival_cars_table'),
                        tooltip = $('<div />', {
                            text: $this.attr('title')
                        });

                    // Если плагин ещё не проинициализирован
                    if (!data) {

                        $this.append($('<table class="compact table-cars cell-border" id="table-list-history-arrival" cellpadding="0" style="width:100%"></table>'))

                        /*
                         * Тут выполняем инициализацию
                        */

                        $(this).data('mt_arrival_cars_table', {
                            target: $this,
                            tooltip: tooltip
                        });

                    }
                });
            },
            show: function () {
                // ПОДХОД
            },
            hide: function () {
                // ПРАВИЛЬНЫЙ
            },
            update: function (content) {
                // !!!
            },
            destroy: function () {

                return this.each(function () {

                    var $this = $(this),
                        data = $this.data('mt_arrival_cars_table');

                    // пространства имён рулят!!11
                    $(window).unbind('.mt_arrival_cars_table');
                    data.tooltip.remove();
                    $this.removeData('mt_arrival_cars_table');
                })

            },
        };

        $.fn.mt_arrival_cars_table = function (method) {

            // Тут пишем функционал нашего плагина
            // логика вызова метода
            if (methods[method]) {
                return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
            } else if (typeof method === 'object' || !method) {
                return methods.init.apply(this, arguments);
            } else {
                $.error('Метод с именем ' + method + ' не существует для jQuery.mt_arrival_cars_table');
            }

        };

    })(jQuery);