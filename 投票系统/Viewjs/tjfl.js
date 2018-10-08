$(function () {
    query()
});

function query() {
    $("#list").hiMallDatagrid({
        url: '/Category/list',
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
            field: "operation",  title: "推荐分类",
            formatter: function (value, row, index) {
                var id = row.Id.toString();
                var html = ["<span class=\"btn-a\">"];
                html.push("<select onchange='changes("+row.Id+")' id='select"+row.Id+"'>");
                html.push("<option value='" + row.tjfl + "' selected>" + row.tjfl + "</option>")
                if (row.tjfl != "暂未推荐")
                {
                    html.push("<option value='暂未推荐'>暂未推荐</option>")
                }
                if (row.tjfl != "一般课题")
                {
                    html.push("<option value='一般课题'>一般课题</option>")
                }
                if (row.tjfl != "重大课题") {
                    html.push("<option value='重大课题'>重大课题</option>")
                }
                if (row.tjfl != "重点课题") {
                    html.push("<option value='重点课题'>重点课题</option>")
                }
                html.push("</select>")
                html.push("</span>");
                return html.join("");
            }
        }
        ]]
    });
}
function getQuery() {
    var result = { state: "进行中" };
    if ($("#name").val() && $("#name").val().length > 0) {
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
function changes(Id)
{
    var vl = document.getElementById("select" + Id).value
    $.ajax({
        type: 'POST',
        url: '/Category/changeTJ',
        data: { Id: Id, tj: vl },
        success: function () { query() },
        dataType: 'json'
    });
}