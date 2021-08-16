$(function () {
  //Start Register Form Validation
  var regFnInput = $('#reg-first-name');
  var regLnInput = $('#reg-last-name');
  var regEmailInput = $('#reg-email');
  var regPassInput = $('#reg-pass');
  var regPassConfInput = $('#reg-conf-pass');
  var regBtn = $('#reg-btn');

  function CheckRegKeyUp() {
    if (!regFnInput.val().trim() || !regLnInput.val().trim() || !regEmailInput.val().trim() || !regPassInput.val().trim() || !regPassConfInput.val().trim() || !DoPassMatch()) {
      regBtn.removeClass('btn-success');
      regBtn.addClass('disabled-btn');
      regBtn.prop('disabled', true);
    } else {
      regBtn.removeClass('disabled-btn');
      regBtn.prop('disabled', false);
      regBtn.addClass('btn-success');
    }
  }

  function DoPassMatch() {
    if (regPassInput.val().trim() === regPassConfInput.val().trim()) {
      return true;
    } else {
      return false;
    }
  }

  regFnInput.on({
    keyup: function () { CheckRegKeyUp(); }
  });
  regLnInput.on({
    keyup: function () { CheckRegKeyUp(); }
  });
  regEmailInput.on({
    keyup: function () { CheckRegKeyUp(); }
  });
  regPassInput.on({
    keyup: function () { CheckRegKeyUp(); DoPassMatch(); }
  });
  regPassConfInput.on({
    keyup: function () { CheckRegKeyUp(); DoPassMatch(); }
  });
  //End Register Form Validation
});