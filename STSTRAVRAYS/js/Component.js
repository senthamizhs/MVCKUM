$(window).load(function () {
    
    "use strict";
    $("#loader").fadeOut("slow");

    setTimeout(function () {
        $('.my_bounceinup').addClass('bounceInUp');
        $('.my_fadeinleft').addClass('fadeInLeft');
        $('.my_fadeinright').addClass('fadeInRight');
    }, 200);

    setTimeout(function () {
        $('.my_flipInX').addClass('flipInX');
        $('.my_bounceInDown').addClass('bounceInDown');
    }, 800);

    setTimeout(function () {
        $('.my_bounce').addClass('bounce');
    },1000);
});

function Counter() {
    $('.counter').counterUp({
        delay: 30, //We can edit delay time...
        time: 600 //We can edit total time to finish counting...
    });
}

