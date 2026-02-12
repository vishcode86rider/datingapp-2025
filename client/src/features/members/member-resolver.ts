import { ResolveFn, Router } from '@angular/router';
import { MemberService } from '../../core/services/member-service';
import { inject } from '@angular/core/primitives/di';
import { Member } from '../../types/member';
import { EMPTY } from 'rxjs';

export const memberResolver: ResolveFn<Member> = (route, state) => {
  const memberService = inject(MemberService);
  const memberiId = route.paramMap.get('id');
  const router = inject(Router);

  if(!memberiId) {
    router.navigateByUrl('/not-found');
    return EMPTY;
  };

  return memberService.getMember(memberiId);
};
