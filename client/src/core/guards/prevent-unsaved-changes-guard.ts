import {  CanDeactivateFn } from '@angular/router';
import { MemberProfile } from '../../features/members/member-profile/member-profile';

export const preventUnsavedChangesGuard: CanDeactivateFn<MemberProfile> = (Component) => {
  if (Component.editForm?.dirty) {
    return confirm('Are you sure you want to continue?. Any unsaved changes will be lost');
  }
  return true;
};
