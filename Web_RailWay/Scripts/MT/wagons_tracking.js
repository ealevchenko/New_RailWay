$(function () {
    //-----------------------------------------------------------------------------------------
    // Объявление глобальных переменных
    //-----------------------------------------------------------------------------------------
    var lang = $.cookie('lang'),
        wagons_tracking = {
            table: null,
            obj_table: null,
            obj: null,
            list: null,
            initTable: function () {
                this.table = $('#table-list-wagons-tracking');
                this.obj = this.table.DataTable({
                    "paging": false,
                    "ordering": true,
                    "info": false,
                    "select": false,
                    "filter": true,
                    "scrollY": "600px",
                    "scrollX": true,
                    language: {
                        decimal: lang == 'en' ? "." : ",",
                        search: lang == 'en' ? "Search" : "Найти вагон:",
                    },
                    jQueryUI: true,
                    "createdRow": function (row, data, index) {
                        //$(row).attr('id', data.id_oper);
                        //if (data.id_oper == this.select) {
                        //    //$(row).addClass('selected');
                        //}
                    },
                    columns: [
                        { data: "nvagon", title: "№ вагона", orderable: false, searchable: false },
                        { data: "st_disl", title: "Код ст. дислокации", orderable: false, searchable: true },
                        { data: "nst_disl", title: "Станция дислокации", orderable: false, searchable: false },
                        { data: "dt", title: "Дата и время операции", width: "150px", orderable: false, searchable: false },

                    ],
                });
                this.obj_table = $('DIV#table-list-wagons-tracking_wrapper');
                //this.obj_table.hide();
            },
            initWagonsTracking: function () {
                getAsyncWagonsTracking(function (result) {
                    wagons_tracking.list = result;
                });
            },
            loadData: function (data) {
                this.list = data;
                this.obj.clear();
                for (i = 0; i < data.length; i++) {
                    this.obj.row.add({
                        "id": data[i].id,
                        "nvagon": data[i].nvagon,
                        "st_disl": data[i].st_disl,
                        "nst_disl": data[i].nst_disl,
                        "dt": data[i].dt,
                    });
                }
                this.obj.draw();
            },
            viewTable: function (data_refresh) {
                this.obj_table.show();
                if (this.list == null | data_refresh == true) {
                    // Обновим данные
                    getAsyncWagonsTracking(function (result) {
                        wagons_tracking.loadData(result);
                    });
                } else {
                    // Не обновим данные

                }
            },
        }

    //-----------------------------------------------------------------------------------------
    // Функции
    //-----------------------------------------------------------------------------------------

    //-----------------------------------------------------------------------------------------
    // Инициализация объектов
    //-----------------------------------------------------------------------------------------
    wagons_tracking.initTable();
    wagons_tracking.viewTable(false);
});