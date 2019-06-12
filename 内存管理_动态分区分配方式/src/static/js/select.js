$(function () {
    selectModel();
});

/*下拉列表选择*/
function selectModel() {
    var $box = $('div.model-box');
    var $option = $('ul.model-select-option', $box);
    var $txt = $('div.model-select-text', $box);
    var speed = 10;
    /**
     * 单击某个下拉列表时，显示当前下拉列表的下拉列表框
     * 并隐藏页面中其他下拉列表
     */
    $txt.on('click', function () {
        var $self = $(this);
        $option.not($self).siblings('ul.model-select-option').slideUp(speed, function () {
            init($self);
        });
        $self.siblings('ul.model-select-option').slideToggle(speed, function () {
            init($self);
        });
        return false;
    });

    // 点击选择，关闭其他下拉框
    /**
     * 为每个下拉列表框中的选项设置默认选中标识 data-selected
     * 点击下拉列表框中的选项时，将选项的 data-option 属性的属性值赋给下拉列表的 data-value 属性，并改变默认选中标识 data-selected
     * 为选项添加 mouseover 事件
     */
    $option.find('li').each(function (index, element) {
        var $self = $(this);
        if ($self.hasClass('selected')) {
            $self.addClass('data-selected');
        }
    }).mousedown(function () {
        $(this).parent().siblings('div.model-select-text').text($(this).text()).attr('data-value', $(this).attr('data-option'));

        $option.slideUp(speed, function () {
            init($(this));
        });
        $(this).addClass('selected data-selected').siblings('li').removeClass('selected data-selected');

        //输出选择的算法
        console.log($("#box").attr("data-value"))

        return false;
    }).mouseover(function () {
        $(this).addClass('selected').siblings('li').removeClass('selected');
    });

    // 点击文档隐藏所有下拉框
    $(document).on('click', function () {
        var $self = $(this);
        $option.slideUp(speed, function () {
            init($self);
        })
    });

    /**
     * 初始化默认选择
     */
    function init(obj) {
        obj.find('li.data-selected').addClass('selected').siblings('li').removeClass('selected');
    }
}