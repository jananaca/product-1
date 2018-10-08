$(function () {
    query()
});

function query() {
    $("#list").hiMallDatagrid({
        url: '/score/jglist',
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
            { field: "name", title: '项目名称' },
            { field: "BH", title: '编号' },
            {
                field: "jssp", title: "技术水平"
            },
            {
                field: "jsnd", title: "技术难度"
            },
              {
                  field: "zscq", title: "知识产权"
              },
              {
                  field: "jjxy", title: "经济效益"
              },
              {
                  field: "shxy", title: "社会效益"
              },
              {
                  field: "tgyy", title: "应用推广"
               },
               { field: "dfhj", title: '打分合计' },

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
    if ($("#category1").val() && $("#category1").val().length > 0) {
        result.category1 = $("#category1").val();
    }
    return result;

}
function change(Id, state) {
    var value = document.getElementById("input" + state + Id).value
    if (state == 1) {
        if (value > 30) {
            alert("输入分值错误");
            return 0;
        }

    }
    else if (state == 2) {
        if (value > 20) {
            alert("输入分值错误");
            return 0;
        }
    }
    else if (state == 3) {
        if (value > 10) {
            alert("输入分值错误");
            return 0;
        }
    }
    else if (state == 4) {
        if (value > 20) {
            alert("输入分值错误");
            return 0;
        }
    }
    else if (state == 5) {
        if (value > 10) {
            alert("输入分值错误");
            return 0;
        }
    }
    else if (state == 6) {
        if (value > 10) {
            alert("输入分值错误");
            return 0;
        }
    }
    $.ajax({
        type: 'POST',
        url: '/score/changepf',
        data: { Id: Id, value: value, state: state },
        success: function () { query() },
        dataType: 'json'
    });

}