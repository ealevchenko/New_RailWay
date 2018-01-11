function getServicesName(service) {
    var value = "?";  //значение по умолчанию умолчанию
    $.ajax({
        type: 'GET',
        url: '/railway/api/log/services/' + service,
        async: false,
        dataType: 'json',
        success: function (data) {
            value = data;
        }
    });
    return value;
}

// Получить список узлов (json) отправки на другие станции
function getStatusServices() {
    var value = null;  //значение по умолчанию умолчанию
    $.ajax({
        url: '/railway/api/log/services/status',
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