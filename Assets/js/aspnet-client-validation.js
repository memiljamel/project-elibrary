import { ValidationService } from 'aspnet-client-validation';

const v = new ValidationService();
v.allowHiddenFields = true;
v.ValidationInputCssClassName = 'is-invalid';
v.ValidationInputValidCssClassName = 'is-valid';
v.ValidationMessageCssClassName = 'invalid-feedback';
v.ValidationMessageValidCssClassName = 'valid-feedback';
v.ValidationSummaryCssClassName = 'validation-summary-errors';
v.ValidationSummaryValidCssClassName = 'validation-summary-valid';
v.bootstrap();
