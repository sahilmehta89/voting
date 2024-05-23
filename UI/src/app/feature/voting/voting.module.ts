import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';



import { VotingApiClient } from 'src/app/core/services/voting/voting-apiclient.service';
import { MaterialModule } from 'src/app/core/material/material-components.module';
import { VotingRoutingModule } from 'src/app/feature/voting/voting-routing.module';
import { CandidatesComponent } from 'src/app/feature/voting/components/candidates/candidates.component';
import { CastVoteComponent } from 'src/app/feature/voting/components/cast-vote/cast-vote.component';
import { VotersComponent } from 'src/app/feature/voting/components/voters/voters.component';
import { VotingAlertingConfigService } from 'src/app/feature/voting/services/voting-alerting-config.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    VotingRoutingModule
  ],
  declarations: [
    CandidatesComponent,
    VotersComponent,
    CastVoteComponent
  ],
  providers: [
    VotingApiClient,
    VotingAlertingConfigService,
  ],
})

export class VotingeModule {
}
