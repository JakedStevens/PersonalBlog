$(function() {
  var elementClicked = false;   
  $('#show-pass-btn').click(function(){
     elementClicked = !elementClicked;
     elementClicked ? $('#login-password').attr('type', 'text') : $('#login-password').attr('type', 'password');
  });

  //Start Login Form Validation
  var loginEmailInput = $('#login-email');
  var loginPassInput = $('#login-password');
  var loginBtn = $('#login-btn');
  var loginEmailValMsg = $('#login-email-val');
  var loginPassValMsg = $('#login-pass-val');

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
  function CheckLoginEmailBlur() {
      loginEmailInput.val() ? loginEmailValMsg.prop('hidden', true) : loginEmailValMsg.prop('hidden', false);
  }

  function CheckLoginPassBlur() {
      loginPassInput.val() ? loginPassValMsg.prop('hidden', true) : loginPassValMsg.prop('hidden', false);
  }
  loginEmailInput.on({
      blur: function() { CheckLoginEmailBlur(); },
      keyup: function() { CheckLoginKeyUp(); }
  });
      loginPassInput.on({
      blur: function() { CheckLoginPassBlur(); },
      keyup: function() { CheckLoginKeyUp(); }
  });
  //End Login Form Validation


  //Start Register Form Validation
  var regFnInput = $('#reg-firstname');
  var regLnInput = $('#reg-lastname');
  var regEmailInput = $('#reg-email');
  var regPassInput = $('#reg-password');
  var regPassConfInput = $('#reg-confirm-password');
  var regBtn = $('#reg-btn');
  var regFnValMsg = $('#reg-fn-val');
  var regLnValMsg = $('#reg-ln-val');
  var regEmailValMsg = $('#reg-email-val');
  var regPassValMsg = $('#reg-pass-val');
  var regCpValMsg = $('#reg-cp-val');
  var regMatchValMsg = $('#reg-match-val');
  var regPlValMsg = $('#reg-Pl-val');

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
  function CheckRegFnBlur() {
      regFnInput.val() ? regFnValMsg.prop('hidden', true) : regFnValMsg.prop('hidden', false);
  }
  function CheckRegLnBlur() {
      regLnInput.val() ? regLnValMsg.prop('hidden', true) : regLnValMsg.prop('hidden', false);
  }
  function CheckRegEmailBlur() {
      regEmailInput.val() ? regEmailValMsg.prop('hidden', true) : regEmailValMsg.prop('hidden', false);
  }
  function CheckRegPassBlur() {
      regPassInput.val() ? regPassValMsg.prop('hidden', true) : regPassValMsg.prop('hidden', false);
  }
  function CheckRegCpBlur() {
      //regPassConfInput.val() ? regCpValMsg.prop('hidden', true) : regCpValMsg.prop('hidden', false);
  }
  function DoPassMatch() {
      if (regPassInput.val().trim() === regPassConfInput.val().trim()) {
          regMatchValMsg.prop('hidden', true);
          return true;
      } else {
          regMatchValMsg.prop('hidden', false);

          return false;
      }   
  }
  function IsPassLongEnough() {
      if (regPassInput.val().trim().length >= 7) {
          regPlValMsg.prop('hidden', true);
          return true;
      } else {
          regPlValMsg.prop('hidden', false);
          return false;
      }
  }
  regFnInput.on({
      blur: function() { CheckRegFnBlur(); },
      keyup: function() { CheckRegKeyUp(); }
  });
  regLnInput.on({
      blur: function() { CheckRegLnBlur(); },
      keyup: function() { CheckRegKeyUp(); }
  });
  regEmailInput.on({
      blur: function() { CheckRegEmailBlur(); },
      keyup: function() { CheckRegKeyUp(); }
  });
  regPassInput.on({
      blur: function() { CheckRegPassBlur(); },
      keyup: function() { CheckRegKeyUp(); IsPassLongEnough();}
  });
  regPassConfInput.on({
      blur: function() { CheckRegCpBlur(); },
      keyup: function() { CheckRegKeyUp(); DoPassMatch(); }
  });
  //End Register Form Validation
});