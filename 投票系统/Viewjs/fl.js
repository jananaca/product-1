$(function () {
    query()
});

function query() {
    $("#list").hiMallDatagrid({
        url: '/Administrator/flList',
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
            { field: "name", title: '分类名' },
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
function upClick(Id) {
    $.ajax({
        type: 'POST',
        url: '/Administrator/delfl',
        data: { id: Id },
        success: function () { query() },
        dataType: 'json'
    });
}