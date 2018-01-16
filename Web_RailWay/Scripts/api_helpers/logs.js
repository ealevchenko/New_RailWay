//$(function () {

    var lang = $.cookie('lang');

    //  
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
        for (var i = 0; i < obj.length; i++) {
            if (obj[i].value == val) return obj[i].text;
        }
        return val;
    }
    // Получить название сервиса или список сервисов
    function getServicesName(service) {
        var value = "?";  //значение по умолчанию умолчанию
        $.ajax({
            type: 'GET',
            url: service != null ? '/railway/api/log/service/name/' + service : '/railway/api/log/service/name',
            async: false,
            dataType: 'json',
            success: function (data) {
                value = data;
            }
        });
        return value;
    }
    // Получить список узлов (json) отправки на другие станции
    function getStatusServices(correct) {
        var value = null;  //значение по умолчанию умолчанию
        var list_serv = getServicesName();
        $.ajax({
            url: '/railway/api/log/services/status',
            type: 'GET',
            async: false,
            dataType: 'json',
            success: function (jsondata) {
                if (correct) {
                    var list_services = [];
                    for (var i = 0; i < jsondata.length; i++) {
                        list_services.push({
                            id: jsondata[i].id,
                            service: jsondata[i].service,
                            servicename: getTextOption(list_serv, jsondata[i].service),
                            start: jsondata[i].start != null ? jsondata[i].start.replace("T", " ") : null,
                            execution: jsondata[i].execution != null ? jsondata[i].execution.replace("T", " ") : null,
                            current: jsondata[i].current,
                            max: jsondata[i].max,
                            min: jsondata[i].min,
                        });
                    }
                }
                value = list_services;
            },
            error: function (x, y, z) {
                LockScreenOff();
                alert(x + '\n' + y + '\n' + z);
            }
        });
        return value;
    }
    // Заполнить компонент tbody списком узлов отправки на другие станции
    function getTbodyStatusServices(tbody, id_select) {
        if (tbody != null) {
            var jsondata = getStatusServices();
            tbody.empty();
            var tab = "";
            for (var i = 0; i < jsondata.length; i++) {
                var status = jsondata[i];
                tab += "<tr id='" + status.id + "' name='services-status' " + (id_select == status.id ? "class='selected'" : "") + ">";
                tab += "<td>" + getServicesName(status.service) + "</td>";
                tab += "<td>" + status.start + "</td>";
                tab += "<td>" + status.execution + "</td>";
                tab += "<td>" + status.current + "</td>";
                tab += "<td>" + status.max + "</td>";
                tab += "<td>" + status.min + "</td>";
                tab += "</tr>";
            }
            tbody.append(tab);
        }
    }

    function getLogServices(correct, service, start, stop) {
        var value = null;  //значение по умолчанию умолчанию
        var list_serv = getServicesName();
        $.ajax({
            url: service != null ? '/railway/api/log/services/' + service + '/' + start.toISOString() + '/' + stop.toISOString() : '/railway/api/log/services/' + start.toISOString() + '/' + stop.toISOString(),
            type: 'GET',
            async: false,
            dataType: 'json',
            success: function (jsondata) {
                if (correct) {
                    var list_log = [];
                    for (var i = 0; i < jsondata.length; i++) {
                        //var services_name = getObjects(list_serv, 'value', jsondata[i].service);
                        list_log.push({
                            id: jsondata[i].id,
                            service: jsondata[i].service,
                            servicename: getTextOption(list_serv,jsondata[i].service),
                            start: jsondata[i].start != null ? jsondata[i].start.replace("T", " ") : null,
                            duration: jsondata[i].duration,
                            code_return: jsondata[i].code_return,
                        });
                    }
                }
                value = list_log;
            },
            error: function (x, y, z) {
                //LockScreenOff();
                alert(x + '\n' + y + '\n' + z);
            }
        });
        return value;
    }

//});