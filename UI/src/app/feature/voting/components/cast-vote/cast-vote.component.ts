import { Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { catchError, forkJoin, Observable, of, Subject, takeUntil } from 'rxjs';

import { AlertingService } from 'src/app/core/alerting/alerting.service';
import { CommonFunctionsService } from 'src/app/core/common/common-functions.service';
import { VotingAlertingConfigService } from '../../services/voting-alerting-config.service';
import { CandidateCreateModel, CandidateViewModel, VotingApiClient, VoteCandidateCreateModel, VoterViewModel } from 'src/app/core/services/voting/voting-apiclient.service';

@Component({
  selector: 'app-cast-vote',
  templateUrl: './cast-vote.component.html',
  styleUrls: ['./cast-vote.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class CastVoteComponent implements OnDestroy, OnInit {
  //Public Variables
  //API Variables
  votersViewModel: VoterViewModel[];
  candidatesViewModel: CandidateViewModel[];
  //UI Variables
  castVoteFormGroup: FormGroup;

  //Private Variables
  //Component Variables
  private _unsubscribeAll: Subject<any> = new Subject<any>();

  constructor(private _formBuilder: FormBuilder, private _alertingService: AlertingService, private _commonFunctionsService: CommonFunctionsService, private _votingApiClient: VotingApiClient, private _votingAlertingConfigService: VotingAlertingConfigService) {
    this.votersViewModel = [];
    this.candidatesViewModel = [];
    this.castVoteFormGroup = this._formBuilder.group({
      voter: [null, Validators.required],
      candidate: [null, Validators.required],
    });
  }

  // -----------------------------------------------------------------------------------------------------
  // @ Lifecycle hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
    this.getAllData();
  }

  /**
   * On destroy
   */
  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next(null);
    this._unsubscribeAll.complete();
  }

  // -----------------------------------------------------------------------------------------------------
  // @ Public methods
  // -----------------------------------------------------------------------------------------------------
  public onClickCastVote(): void {
    if (this.castVoteFormGroup.valid) {
      this.saveCastVote();
    }
  }

  public onClickCancel(): void {
    this.resetCastVoteForm();
  }

  // -----------------------------------------------------------------------------------------------------
  // @ Private methods
  // -----------------------------------------------------------------------------------------------------
  private getAllData(): void {
    const apiRequests$: Observable<any>[] = [];

    const votersApiRequest$ = this._votingApiClient.voterAll().pipe(
      catchError(error => of(error))
    );
    apiRequests$.push(votersApiRequest$);

    const candidatesApiRequest$ = this._votingApiClient.candidateAll().pipe(
      catchError(error => of(error))
    );
    apiRequests$.push(candidatesApiRequest$);

    forkJoin(apiRequests$)
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe({
        next: (result) => {
          this.getVoters(result[0]);
          this.getCandidates(result[1]);
        },
        error: (e) => {
          this.onErrorInGetAllData(e);
        }
      });
  }

  private getVoters(votersViewModel: VoterViewModel[]): void {
    this.votersViewModel = votersViewModel;
  }

  private getCandidates(candidatesViewModel: CandidateViewModel[]): void {
    this.candidatesViewModel = candidatesViewModel;
  }

  private onErrorInGetAllData(e: any) {
    console.log(e);
    this._alertingService.error(this._votingAlertingConfigService.getAlertingConfigForErrorInGettingVotersAndCandidates());
  }

  private saveCastVote(): void {
    const voteCandidateCreateModel = new VoteCandidateCreateModel();
    voteCandidateCreateModel.voterId = this.castVoteFormGroup.get('voter')?.value,
      voteCandidateCreateModel.candiateId = this.castVoteFormGroup.get('candidate')?.value,

      this._votingApiClient.voteCandidate(voteCandidateCreateModel)
        .pipe(takeUntil(this._unsubscribeAll))
        .subscribe({
          next: (apiResult: number) => {
            this.resetCastVoteForm();
            this._alertingService.success(this._votingAlertingConfigService.getAlertingConfigForSuccessInCastVote());
            this.getAllData();
          },
          error: (e) => {
            this.onErrorSaveCastVote(e);
          },
        });
  }

  private onErrorSaveCastVote(e?: any): void {
    if (this._commonFunctionsService.isObjectNotNull(e)) {
      console.error(e);
    }
    this.resetCastVoteForm();
    this._alertingService.error(this._votingAlertingConfigService.getAlertingConfigForErrorInCastVote());
  }

  private resetCastVoteForm(): void {
    this.castVoteFormGroup.reset();
  }
}

