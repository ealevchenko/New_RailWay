//---------------------------------------------
// Station
//---------------------------------------------
// Вернуть название станци
function getStationName(id) {
    var value = "?";  //значение по умолчанию умолчанию
    $.ajax({
        type: 'GET',
        url: '/railway/api/rw/station_name/' + id,
        async: false,
        dataType: 'json',
        success: function (data) {
            value = data;
        }
    });
    return value;
}
// Заполнить компонент select списком станций
function selectStations(name, value) {
    $.ajax({
        url: '/railway/api/rw/stations/view/amkr',
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (jsondata) {
            // Заполним селект станциями
            var options = [];
            for (i = 0; i < jsondata.length; i++) {
                var station = jsondata[i];
                options.push("<option value='" + station.id + "' >" + station.name_ru + "</option>");
            }
            $('select[name ="' + name + '"]')
                .append(options.join(""))
                .val(value)
                .selectmenu("refresh");
        },
        error: function (x, y, z) {
            LockScreenOff();
            alert(x + '\n' + y + '\n' + z);
        }
    });
}
// Заполнить компонент select списком станций 
function selectListStations(obj, value, cng, exceptions) {
    $.ajax({
        url: '/railway/api/rw/stations/view/amkr',
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (jsondata) {
            var options = [];
            for (i = 0; i < jsondata.length; i++) {
                var station = jsondata[i];
                if (exceptions != null) {
                    if (exceptions.indexOf(station.id) == -1) {
                        options.push("<option value='" + station.id + "' >" + station.name_ru + "</option>");
                    }
                } else {
                    options.push("<option value='" + station.id + "' >" + station.name_ru + "</option>");
                }
            }
            obj.empty();
            obj.selectmenu({
                icons: { button: "ui-icon ui-icon-circle-triangle-s" },
                width: 300,
                change: cng,
            });
            // Заполним селект станциями
            obj.append(options.join(""))
                .val(value)
                .selectmenu("refresh");
        },
        error: function (x, y, z) {
            LockScreenOff();
            alert(x + '\n' + y + '\n' + z);
        }
    });
}
//---------------------------------------------
// SideStation
//---------------------------------------------
// Вернуть изображение стороны станции
function getImageSideStation(side) {
    var value = "";
    if (side == 1) {
        value = '<img src="Image/railway/even.png" />';
    }
    if (side == 0) {
        value = '<img src="Image/railway/odd.png" />';
    }
    return value;
}
// Вернуть определение стороны станции
function getSideStationName(side) {
    var value = "?";  //значение по умолчанию умолчанию
    $.ajax({
        type: 'GET',
        url: '/railway/api/rw/side/' + Number(side),
        async: false,
        dataType: 'json',
        success: function (data) {
            value = data;
        }
    });
    return value;
}
// Заполнить компонент select списком сторон 
function selectListSideStation(obj, value, cng) {
    $.ajax({
        url: '/railway/api/rw/side',
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (jsondata) {
            var options = [];
            for (i = 0; i < jsondata.length; i++) {
                var side = jsondata[i];
                options.push("<option value='" + side.value + "' >" + side.text + "</option>");
            }
            obj.selectmenu({
                icons: { button: "ui-icon ui-icon-circle-triangle-s" },
                width: 300,
                change: cng,
            });
            // Заполним селект станциями
            obj.empty();
            obj.append(options.join(""))
                .val(Number(value))
                .selectmenu("refresh");
        },
        error: function (x, y, z) {
            LockScreenOff();
            alert(x + '\n' + y + '\n' + z);
        }
    });
}
//---------------------------------------------
// TransferType
//---------------------------------------------
// Вернуть изображение типа передачи составов
function getImageTransferType(type) {
    var value = "";
    if (type == 0) {
        value = '<img src="Image/railway/type_railway.png" />';
    }
    if (type == 1) {
        value = '<img src="Image/railway/type_input.png" />';
    }
    if (type == 2) {
        value = '<img src="Image/railway/type_output.png" />';
    }
    return value;
}
// Вернуть определение типа передачи составов
function getTransferTypeName(type) {
    var value = "?";  //значение по умолчанию умолчанию
    $.ajax({
        type: 'GET',
        url: '/railway/api/rw/send_transfer/type/' + type,
        async: false,
        dataType: 'json',
        success: function (data) {
            value = data;
        }
    });
    return value;
}
// Заполнить компонент select списком типов отправки
function selectListTransferType(obj, value, cng) {
    $.ajax({
        url: '/railway/api/rw/send_transfer/type',
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (jsondata) {
            var options = [];
            for (i = 0; i < jsondata.length; i++) {
                var side = jsondata[i];
                options.push("<option value='" + side.value + "' >" + side.text + "</option>");
            }
            obj.selectmenu({
                icons: { button: "ui-icon ui-icon-circle-triangle-s" },
                width: 300,
                change: cng,
            });
            // Заполним селект станциями
            obj.empty();
            obj.append(options.join(""))
                .val(value)
                .selectmenu("refresh");
        },
        error: function (x, y, z) {
            LockScreenOff();
            alert(x + '\n' + y + '\n' + z);
        }
    });
}
//---------------------------------------------
// StationsNodes
//---------------------------------------------
// Получить список id станций имеющих узел с указанной станцией
function getListIDStationsOfSendStationsNodes(id, id_exceptions) {
    var value = null;  //значение по умолчанию умолчанию
    $.ajax({
        url: '/railway/api/rw/stations_nodes/send/station/' + id,
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (jsondata) {
            var id_stations = [];
            id_stations.push(id);
            for (i = 0; i < jsondata.length; i++) {
                if (jsondata[i].id_station_on != id_exceptions) {
                    id_stations.push(jsondata[i].id_station_on);
                }
            }
            value = id_stations;
        },
        error: function (x, y, z) {
            LockScreenOff();
            alert(x + '\n' + y + '\n' + z);
        }
    });
    return value
}

function getListIDStationsOfArrivalStationsNodes(id, id_exceptions) {
    var value = null;  //значение по умолчанию умолчанию
    $.ajax({
        url: '/railway/api/rw/stations_nodes/arrival/station/' + id,
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (jsondata) {
            var id_stations = [];
            id_stations.push(id);
            for (i = 0; i < jsondata.length; i++) {
                if (jsondata[i].id_station_from != id_exceptions) {
                    id_stations.push(jsondata[i].id_station_from);
                }
            }
            value = id_stations;
        },
        error: function (x, y, z) {
            LockScreenOff();
            alert(x + '\n' + y + '\n' + z);
        }
    });
    return value
}

// Получить список узлов (json) отправки на другие станции
function getSendStationsNodes(id) {
    var value = null;  //значение по умолчанию умолчанию
    $.ajax({
        url: '/railway/api/rw/stations_nodes/send/station/' + id,
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (jsondata) {
            value = jsondata;
        },
        error: function (x, y, z) {
            LockScreenOff();
            alert(x + '\n' + y + '\n' + z);
        }
    });
    return value;
}
// Заполнить компонент tbody списком узлов отправки на другие станции
function getTbodySendStationsNodes(id, tbody, id_select) {
    if (tbody != null) {
        var jsondata = getSendStationsNodes(id);
        tbody.empty();
        var tab = "";
        for (var i = 0; i < jsondata.length; i++) {
            var node = jsondata[i];
            tab += "<tr id='" + node.id + "' name='send-station' " + (id_select == node.id ? "class='selected'" : "") + ">";
            tab += "<td>" + getStationName(node.id_station_from) + "</td>";
            tab += "<td>" + getImageSideStation(node.side_station_from) + "</td>";
            tab += "<td>" + getImageTransferType(node.transfer_type) + "</td>";
            tab += "<td>" + getImageSideStation(node.side_station_on) + "</td>";
            tab += "<td>" + getStationName(node.id_station_on) + "</td>";
            tab += "<td>" + node.nodes + "</td>";
            tab += "</tr>";
        }
        tbody.append(tab);
    }
}
// Получить список узлов (json) принятия с других станций
function getArrivalStationsNodes(id) {
    var value = null;  //значение по умолчанию умолчанию
    $.ajax({
        url: '/railway/api/rw/stations_nodes/arrival/station/' + id,
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (jsondata) {
            value = jsondata;
        },
        error: function (x, y, z) {
            LockScreenOff();
            alert(x + '\n' + y + '\n' + z);
        }
    });
    return value;
}
// Заполнить компонент tbody списком узлов принятия с других станций
function getTbodyArrivalStationsNodes(id, tbody, id_select) {
    if (tbody != null) {
        var jsondata = getArrivalStationsNodes(id);
        tbody.empty();
        var tab = "";
        for (var i = 0; i < jsondata.length; i++) {
            var node = jsondata[i];
            tab += "<tr id='" + node.id + "' name='arrival-station' " + (id_select == node.id ? "class='selected'" : "") + ">";
            tab += "<td>" + getStationName(node.id_station_from) + "</td>";
            tab += "<td>" + getImageSideStation(node.side_station_from) + "</td>";
            tab += "<td>" + getImageTransferType(node.transfer_type) + "</td>";
            tab += "<td>" + getImageSideStation(node.side_station_on) + "</td>";
            tab += "<td>" + getStationName(node.id_station_on) + "</td>";
            tab += "<td>" + node.nodes + "</td>";
            tab += "</tr>";
        }
        tbody.append(tab);
    }
}


