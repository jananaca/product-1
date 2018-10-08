$(function () {
    query()
});

function query() {
    $("#list").hiMallDatagrid({
        url: '/Administrator/yhglList',
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
        queryParams: getQuery(),
        toolbar: /*"#goods-datagrid-toolbar",*/'',
        columns:
        [[
            { field: "Id", hidden: true },
            { field: "username", title: '用户名' },
            { field: "name", title: '姓名' },
            { field: "sex", title: '性别' },
            { field: "department", title: '部门' },
            { field: "group", title: '用户组' },
            { field: "state", title: '状态' },

        {
            field: "operation", operation: true, title: "操作",
            formatter: function (value, row, index) {
                var id = row.Id.toString();
                if (row.state == '正常') {
                    var html = ["<span class=\"btn-a\">"];
                    html.push("<a onclick=\"del('" + id + "');\">删除用户</a>");
                    html.push("<a onclick=\"dj('" + id + "');\">冻结用户</a>");
                    html.push("</span>");
                    return html.join("");
                }
                else
                {
                    var html = ["<span class=\"btn-a\">"];
                    html.push("<a onclick=\"del('" + id + "');\">删除用户</a>");
                    html.push("<a onclick=\"hf('" + id + "');\">恢复用户</a>");
                    html.push("</span>");
                    return html.join("");
                }
            }
        }
        ]]
    });
}
function del(Id)
{
    $.ajax({
        type: 'POST',
        url: '/Administrator/updateyh',
        data: {id:Id,state:3},
        success: function () { alert("删除成功"); query() },
        dataType: 'json'
    });
}
function dj(Id) {
    $.ajax({
        type: 'POST',
        url: '/Administrator/updateyh',
        data: { id: Id, state: 2 },
        success: function () { alert("冻结成功"); query() },
        dataType: 'json'
    });
}
function hf(Id) {
    $.ajax({
        type: 'POST',
        url: '/Administrator/updateyh',
        data: { id: Id, state: 1 },
        success: function () { alert("恢复成功"); query() },
        dataType: 'json'
    });
}
function getQuery() {
    var result = { };
    if ($("#name").val() && $("#name").val().length > 0) {
        result.name = $("#name").val();
    }
    if ($("#username").val() && $("#username").val().length > 0) {
        result.username = $("#username").val();
    }
    if ($("#groups").val() && $("#groups").val().length > 0) {
        result.groups = $("#groups").val();
    }
    if ($("#state").val() && $("#state").val().length > 0) {
        result.state = $("#state").val();
    }
    if ($("#department").val() && $("#department").val().length > 0) {
        result.department = $("#department").val();
    }
    return result;

}