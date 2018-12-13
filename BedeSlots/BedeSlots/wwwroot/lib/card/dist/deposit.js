$(function () {

    var cardNumber = $('#Number');
    var cardNumberField = $('#card-number-field');
    var CVV = $("#Cvv");
    var CVVField = $('#card-cvv-field');
    var mastercard = $("#mastercard");
    var confirmButton = $('#confirm-purchase');
    var visa = $("#visa");
    var amex = $("#amex");

    cardNumber.payform('formatCardNumber');
    CVV.payform('formatCardCVC');


    cardNumber.keyup(function () {

        amex.removeClass('transparent');
        visa.removeClass('transparent');
        mastercard.removeClass('transparent');

        if ($.payform.validateCardNumber(cardNumber.val()) == false) {
            cardNumberField.addClass('has-error');
        } else {
            cardNumberField.removeClass('has-error');
            cardNumberField.addClass('has-success');
        }

        if ($.payform.parseCardType(cardNumber.val()) == 'visa') {
            mastercard.addClass('transparent');
            amex.addClass('transparent');
        } else if ($.payform.parseCardType(cardNumber.val()) == 'amex') {
            mastercard.addClass('transparent');
            visa.addClass('transparent');
        } else if ($.payform.parseCardType(cardNumber.val()) == 'mastercard') {
            amex.addClass('transparent');
            visa.addClass('transparent');
        }
    });

    CVV.keyup(function () {
        if ($.payform.validateCardCVC(CVV.val()) == false) {
            CVVField.addClass('has-error');
        } else {
            CVVField.removeClass('has-error');
            CVVField.addClass('has-success');
        }
    });

    confirmButton.click(function (e) {

        e.preventDefault();

        var isCardValid = $.payform.validateCardNumber(cardNumber.val());
        var isCvvValid = $.payform.validateCardCVC(CVV.val());

        if (isCardValid && isCvvValid) {

            const $creditCardForm = $('#credit-card-form');
            const dataToSend = $creditCardForm.serialize();

            $.post($creditCardForm.attr('action'), dataToSend, function (result) {
                $('#status-message').html(result);
            }); 

            modalClear();
            $('#credid-card-modal').modal('hide');
        }
    });

    function modalClear() {
        $('#credid-card-modal')
            .find("input")
            .val('')
            .end()
    }

    $('.datepicker').datepicker({
        startDate: '0y'
    });
});

$('#open-card-modal').click(function () {
    $('#credid-card-modal').modal('show');
});
