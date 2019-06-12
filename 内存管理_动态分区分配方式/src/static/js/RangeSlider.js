$.fn.RangeSlider = function (cfg) {
    this.sliderCfg = {
        min: cfg && !isNaN(parseFloat(cfg.min)) ? Number(cfg.min) : null,
        max: cfg && !isNaN(parseFloat(cfg.max)) ? Number(cfg.max) : null,
        step: cfg && Number(cfg.step) ? cfg.step : 1,
        callback: cfg && cfg.callback ? cfg.callback : null
    };

    var $input = $(this);
    var min = this.sliderCfg.min;
    var max = this.sliderCfg.max;
    var step = this.sliderCfg.step;
    var callback = this.sliderCfg.callback;

    $input.attr('min', min)
        .attr('max', max)
        .attr('step', step);

    $input.bind("input", function (e) {
        $input.attr('value', this.value);
        $input.css('background', 'linear-gradient(to right, #059CFA, white ' + (this.value - 200) / 8 + '%, white)');
                            //其中要将this.value进行从[200,1000]到[0,100]的函数映射

        if ($.isFunction(callback)) {
            callback(this);
        }
    });
};