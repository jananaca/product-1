$(function () {
    query()
});

function query() {
    $("#list").hiMallDatagrid({
        url: '/Boss/tplist1',
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
             { field: "tjfl", title: '推荐分类' },
             { field: "ps", title: '票数' },
        {
            field: "operation", title: "确认分类",
            formatter: function (value, row, index) {
                var id = row.Id.toString();
                var html = ["<span class=\"btn-a\">"];
                html.push("<select onchange='changes(" + row.Id + ")' id='select" + row.Id + "'>");
                if (row.qrfl == row.tjfl) {
                    html.push("<option value='同意' selected>同意</option>")
                    html.push("<option value='反对' >反对</option>")
                }
                else
                {
                    html.push("<option value='同意' >同意</option>")
                    html.push("<option value='反对' selected>反对</option>")
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
function changes(Id) {
    var vl = document.getElementById("select" + Id).value
    $.ajax({
        type: 'POST',
        url: '/Boss/changeTP',
        data: { Id: Id, tj: vl },
        success: function () { query() },
        dataType: 'json'
    });
}