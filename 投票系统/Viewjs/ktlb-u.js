$(function () {
    query()
});

function query() {
    $("#list").hiMallDatagrid({
        url: '/upproduct/list',
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
            { field: "BH", title: '编号' },
            { field: "FZR", title: '负责人' },
            { field: "Category", title: '分类' },
            { field: "addDate", title: '添加时间' },
            { field: "tjfl", title: '推荐分类' },
            { field: "qrfl", title: '确认分类' },
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