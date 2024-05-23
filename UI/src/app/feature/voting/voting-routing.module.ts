import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CandidatesComponent } from 'src/app/feature/voting/components/candidates/candidates.component';
import { VotersComponent } from 'src/app/feature/voting/components/voters/voters.component';
import { CastVoteComponent } from 'src/app/feature/voting/components/cast-vote/cast-vote.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'candidates',
  },
  {
    path: 'candidates',
    component: CandidatesComponent,
  },
  {
    path: 'voters',
    component: VotersComponent
  },
  {
    path: 'cast-vote',
    component: CastVoteComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class VotingRoutingModule {
}
