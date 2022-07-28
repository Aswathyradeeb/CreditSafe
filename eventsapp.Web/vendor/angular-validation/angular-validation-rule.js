(function () {
    angular
      .module('validation.rule', ['validation'])
      .config(['$validationProvider', function ($validationProvider) {
          var expression = {
              required: function (value) {
                  return !!value;
              },
              participantMinCount: function (value, scope, element, attrs, param) {
                  debugger
                  return value && value >= parseInt(attrs.min);
              },
              participantMaxCount: function (value, scope, element, attrs, param) {
                  debugger
                  return value && value <= parseInt(attrs.max);
              },
              null: function (value) {
                  return true;
              },
              url: /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/,
              email: /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/,
              number: /^\d+$/,
              password: /([.!#$%&*=()-+_~.;,'~^`"|\d\w])?(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[`~$@$!^#.%*?+=&():;\-"'\\-_{}\[\]><|,/])[A-Za-z\d$@$!=(){}%*?&#,`.;:'^~"-_|\w]{7,10}/,
              phoneNumber: /^([+]?)[0-9]{1}[0-9]{9,15}$/,
              phoneNumberWithCountryCode: /^[1-9]{1}[0-9]{10,15}$/,
              emiratesId: /^784-[0-9]{4}-[0-9]{7}-[0-9]{1}$/,
              workPhone: /^[0]{1}[0-9]{8,9}/,
              isbn: /^(?:(?:-1[03])?:?)?(?=[0-9X]{10}$|(?=(?:[0-9]+[- ]){3})[- 0-9X]{13}$|97[89][0-9]{10}$|(?=(?:[0-9]+[- ]){4})[- 0-9]{17}$)(?:97[89][- ]?)?[0-9]{1,5}[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9X]$/,
              minlength: function (value, scope, element, attrs, param) {
                  return value && value.length >= param;
              },
              maxlength: function (value, scope, element, attrs, param) {
                  return !value || value.length <= param;
              }
          };

          var defaultMsg = {
              required: {
                  error: {
                      nameEn: 'This field is Required!',
                      nameAr: 'يجب تعبئة هذا الحقل'
                  }
              },
              participantMaxCount: {
                  error: {
                      nameEn: 'Maximum number of visitors per registration – 5',
                      nameAr: 'الحد الأقصى للتسجيل هو 5 أشخاص'
                  }
              },
              participantMinCount: {
                  error: {
                      nameEn: 'Minimum number of visitors per registration – 1',
                      nameAr: 'الحد الأدنى للتسجيل هو 1 أشخاص'
                  }
              },
              null: {
                  error: {
                      nameEn: '',
                      nameAr: ''
                  }
              },
              phoneNumber: {
                  error: {
                      nameEn: 'This should be Phone Number!',
                      nameAr: 'يجب أن تكون صيغة الهاتف صحيحة'
                  }
              },
              phoneNumberWithCountryCode: {
                  error: {
                      nameEn: 'This should be Phone Number! (971xxxxxxxxx)',
                      nameAr: 'يجب أن يكون هذا رقم الهاتف! (971xxxxxxxxx)'
                  }
              },
              emiratesId: {
                  error: {
                      nameEn: 'This should be valid Emirate ID!',
                      nameAr: 'يجب أن يكون هذا هو بطاقة هوية الإمارات'
                  }
              },
              workPhone: {
                  error: {
                      nameEn: 'This should be Work Phone!',
                      nameAr: 'يجب أن تكون صيغة الهاتف صحيحة'
                  }
              },
              isbn: {
                  error: {
                      nameEn: 'This should be ISBN!',
                      nameAr: 'رقم ردمك غير صحيح'
                  }
              },
              url: {
                  error: {
                      nameEn: 'This should be website link!',
                      nameAr: 'هذا ينبغي أن يكون رابط الموقع'
                  }
              },
              email: {
                  error: {
                      nameEn: 'This should be Email',
                      nameAr: 'يجب أن تكون صيغة البريد الإلكتروني صحيحة'
                  }
              },
              number: {
                  error: {
                      nameEn: 'This should be number',
                      nameAr: 'يجب أن يكون هذا الرقم'
                  }
              },
              password: {
                  error: {
                      nameEn: 'Password should be minimum 7 and maximum 10 characters at least 1 uppercase alphabet, 1 lowercase alphabet, 1 number and 1 special character',
                      nameAr: 'يجب أن يتألف من 7 إلى 10 ويحتوي على الأقل 1 حرف كبير، 1 حرف صغير، 1 رقم، 1 رمز'
                  }
              },
              minlength: {
                  error: 'This should be longer',
                  success: 'Long enough!'
              },
              maxlength: {
                  error: 'This should be shorter',
                  success: 'Short enough!'
              }
          };
          $validationProvider.setExpression(expression).setDefaultMsg(defaultMsg);
      }]);
}).call(this);