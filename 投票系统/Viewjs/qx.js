$(function () {
    query()
});

function query() {
    $("#list").hiMallDatagrid({
        url: '/Administrator/qxList',
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
            { field: "name", title: '角色名' },
             {
                 field: "addReview", title: "新建课题", nowrap: false,align:'center',width:200,
                  formatter: function (value, row, index) {
                      var id = row.Id.toString();
                      if (row.addReview==1) {
                          var html = [];
                          html.push("<img src='/image/yes.png' style='width:15%; margin-left:42.5%' onclick=\"change('" + id + "',this," + row.addReview + ",1);\">");
                          return html.join("");
                      }
                      else {
                          var html = [];
                          html.push("<img src='/image/error.png' style='width:15%; margin-left:42.5%' onclick=\"change('" + id + "',this," + row.addReview + ",1);\">");
                          return html.join("");
                      }
                  }
             },
            {
                field: "Review", title: "投票", nowrap: false, align: 'center', width: 200,
                formatter: function (value, row, index) {
                    var id = row.Id.toString();
                    if (row.Review == 1) {
                        var html = [];
                        html.push("<img src='/image/yes.png' style='width:15%; margin-left:42.5%' onclick=\"change('" + id + "',this," + row.Review + ",2);\"/>");
                        return html.join("");
                    }
                    else {
                        var html = [];
                        html.push("<img src='/image/error.png' style='width:15%; margin-left:42.5%' onclick=\"change('" + id + "',this," + row.Review + ",2);\"/>");
                        return html.join("");
                    }
                }
            },
            {
                field: "Administrator", title: "网络管理员", nowrap: false, align: 'center', width: 200,
                formatter: function (value, row, index) {
                    var id = row.Id.toString();
                    if (row.Administrator == 1) {
                        var html = [];
                        html.push("<img src='/image/yes.png' style='width:15%; margin-left:42.5%' onclick=\"change('" + id + "',this," + row.Administrator + ",3);\"/>");
                        return html.join("");
                    }
                    else {
                        var html = [];
                        html.push("<img src='/image/error.png' style='width:15%; margin-left:42.5%' onclick=\"change('" + id + "',this," + row.Administrator + ",3);\"/>");
                        return html.join("");
                    }
                }
            },
            {
                field: "Technical", title: "课题管理", nowrap: false, align: 'center', width: 200,
                formatter: function (value, row, index) {
                    var id = row.Id.toString();
                    if (row.Technical == 1) {
                        var html = [];
                        html.push("<img src='/image/yes.png' style='width:15%; margin-left:42.5%' onclick=\"change('" + id + "',this," + row.Technical + ",4);\"/>");
                        return html.join("");
                    }
                    else {
                        var html = [];
                        html.push("<img src='/image/error.png' style='width:15%; margin-left:42.5%' onclick=\"change('" + id + "',this," + row.Technical + ",4);\"/>");
                        return html.join("");
                    }
                }
            },
              {
                  field: "TJ", title: "推荐", nowrap: false, align: 'center', width: 200,
                  formatter: function (value, row, index) {
                      var id = row.Id.toString();
                      if (row.TJ == 1) {
                          var html = [];
                          html.push("<img src='/image/yes.png' style='width:15%; margin-left:42.5%' onclick=\"change('" + id + "',this," + row.TJ + ",5);\"/>");
                          return html.join("");
                      }
                      else {
                          var html = [];
                          html.push("<img src='/image/error.png' style='width:15%; margin-left:42.5%' onclick=\"change('" + id + "',this," + row.TJ + ",5);\"/>");
                          return html.join("");
                      }
                  }
              },
              {
                  field: "ZG", title: "总工", nowrap: false, align: 'center', width: 200,
                  formatter: function (value, row, index) {
                      var id = row.Id.toString();
                      if (row.ZG == 1) {
                          var html = [];
                          html.push("<img src='/image/yes.png' style='width:15%; margin-left:42.5%' onclick=\"change('" + id + "',this," + row.ZG + ",6);\"/>");
                          return html.join("");
                      }
                      else {
                          var html = [];
                          html.push("<img src='/image/error.png' style='width:15%; margin-left:42.5%' onclick=\"change('" + id + "',this," + row.ZG + ",6);\"/>");
                          return html.join("");
                      }
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
function change(Id,th,value,index) {
    $.ajax({
        type: 'POST',
        url: '/Administrator/qxChange',
        data: { id: Id, index:index },
        success: function () { query() },
        dataType: 'json'
    });
}
function upClick(Id)
{
    $.ajax({
        type: 'POST',
        url: '/Administrator/delqx',
        data: { Id: Id },
        success: function () { query() },
        dataType: 'json'
    });
}