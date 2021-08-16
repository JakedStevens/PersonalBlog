$(function () {
  // Show Password Button
  var elementClicked = false;
  $('#show-pass-btn').click(function () {
    elementClicked = !elementClicked;
    elementClicked ? $('#login-password').attr('type', 'text') : $('#login-password').attr('type', 'password');
  });

  //Start Login Form Validation
  var loginEmailInput = $('#login-email');
  var loginPassInput = $('#login-password');
  var loginBtn = $('#login-btn');

  function CheckLoginKeyUp() {
    if (!loginEmailInput.val() || !loginPassInput.val()) {
      loginBtn.removeClass('btn-success');
      loginBtn.addClass('disabled-btn');
      loginBtn.prop('disabled', true);
    } else {
      loginBtn.removeClass('disabled-btn');
      loginBtn.prop('disabled', false);
      loginBtn.addClass('btn-success');
    }
  }
  loginEmailInput.on({
    keyup: function () { CheckLoginKeyUp(); }
  });
  loginPassInput.on({
    keyup: function () { CheckLoginKeyUp(); }
  });
  //End Login Form Validation
});