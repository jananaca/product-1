$(function () {
    query()
});

function query() {
    $("#list").hiMallDatagrid({
        url: '/Administrator/yhzList',
        nowrap: false,
        rownumbers: true,
        NoDataMsg: '没有找到符合条件的数据',
        border: false,
        fit: true,
        fitColumns: true,
        pagination: true,
        idField: "Id",
        pageSize: 10,
        pageNumber: 1,
        queryParams: {},
        toolbar: /*"#goods-datagrid-toolbar",*/'',
        columns:
        [[
            { field: "Id", hidden: true },
            { field: "name", title: '用户组名' },
             {
                 field: "pri", title: "角色", nowrap: false, align: 'center', width: 200,
                 formatter: function (value, row, index) {
                     var id = row.Id.toString();
                     var html = ["<select onchange='changes(" + id + ")' id='select" + id + "'>"]
                     html.push("<option selected value=" + row.pri + ">" + row.pri + "</option>")
                     var l = row.ap.split(',')
                     for (var i = 0; i < l.length;i++)
                     {
                         if (l[i] != row.pri)
                         {
                             html .push("<option value=" + l[i]+ ">" + l[i]+ "</option>")
                         }
                     }
                     html.push("</select>")
                     return html.join("");
                 }
             },
              {
                  field: "operation", operation: true, title: "操作",
                  formatter: function (value, row, index) {
                      var id = row.Id.toString();
                      var html = ["<span class=\"btn-a\">"];
                      html.push("<a onclick=\"upClick('" + id + "');\">删除</a>");
                      html.push("</span>");
                      return html.join("");
                  }
              }
        ]]
    });
}
function changes(Id) {
    var vl=document.getElementById("select"+Id).value
    $.ajax({
        type: 'POST',
        url: '/Administrator/yhzChange',
        data: { id: Id,vl: vl },
        success: function () { query() },
        dataType: 'json'
    });
}
function upClick(Id) {
    $.ajax({
        type: 'POST',
        url: '/Administrator/delyhz',
        data: { id: Id },
        success: function () { query() },
        dataType: 'json'
    });
}