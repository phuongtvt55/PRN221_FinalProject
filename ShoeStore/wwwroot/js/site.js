// Dropdown on mouse hover
$(document).ready(function () {
    function toggleNavbarMethod() {
        if ($(window).width() > 992) {
            $('.navbar .dropdown').on('mouseover', function () {
                $('.dropdown-toggle', this).trigger('click');
            }).on('mouseout', function () {
                $('.dropdown-toggle', this).trigger('click').blur();
            });
        } else {
            $('.navbar .dropdown').off('mouseover').off('mouseout');
        }
    }
    toggleNavbarMethod();
    $(window).resize(toggleNavbarMethod);
});


// Back to top button
$(window).scroll(function () {
    if ($(this).scrollTop() > 100) {
        $('.back-to-top').fadeIn('slow');
    } else {
        $('.back-to-top').fadeOut('slow');
    }
});
$('.back-to-top').click(function () {
    $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
    return false;
});



// Product Quantity
/*$('.quantity button').on('click', function () {
    var button = $(this);
    var oldValue = button.parent().parent().find('input').val();
    console.log(oldValue);
    if (button.hasClass('btn-plus')) {
        var newVal = parseFloat(oldValue) + 1;
        console.log(newVal);
        if (newVal > 99) {
            alert("Too high");
            newVal--;
        }

    } else {
        if (oldValue > 0) {
            var newVal = parseFloat(oldValue) - 1;
        } else {
            newVal = 0;
        }
    }
    button.parent().parent().find('input').val(newVal);
});*/

/*$('.input-quantity-cart').on('blur', function () {
    var button = $(this);
    var oldValue = button.parent().parent().find('input').val();
    if (oldValue > 99) {
        alert("Too high");
        oldValue = 99;
    }
    button.parent().parent().find('input').val(oldValue);
});*/

