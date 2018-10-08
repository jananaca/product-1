$(function () {
   query()
});

function query() {
    $("#list").hiMallDatagrid({
        url: '/Administrator/list',
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
            { field: "Name", title: '项目名称' },
            { field: "FZR", title: '负责人' },
            { field: "Category", title: '分类' },
            { field: "addDate", title: '添加时间' },
        {
            field: "operation", operation: true, title: "操作",
            formatter: function (value, row, index) {
                var id = row.Id.toString();
                var html = ["<span class=\"btn-a\">"];
                html.push("<a onclick=\"upClick('" + id + "');\">上传进度</a>");
                html.push("</span>");
                return html.join("");
            }
        }
        ]]
    });
}
function getQuery()
{
    var result = { state: "进行中" };
    if ($("#name").val()&&$("#name").val().length > 0)
    {
        result.name = $("#name").val();
    }
    if ($("#fzr").val() && $("#fzr").val().length > 0) {
        result.fzr = $("#fzr").val();
    }
    if ($("#category").val() && $("#category").val().length > 0) {
        result.category = $("#category").val();
    }
    return result;

}