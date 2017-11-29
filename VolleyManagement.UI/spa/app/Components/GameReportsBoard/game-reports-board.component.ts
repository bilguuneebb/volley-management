import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/toPromise';

import { TournamentMetadataJson } from '../../Models/TournamentMetadataJson/TournamentMetadataJson';
import { TournamentDataService } from '../../Services/tournament-data.service';


@Component({
    selector: 'vm-game-reports-board',
    templateUrl: './game-reports-board.component.html',
    styleUrls: ['./game-reports-board.component.scss']
})
export class GameReportsBoardComponent implements OnInit {

    reportsStates = {
        standings: true,
        pivot: true,
        schedule: true
    };

    pivotId: number;
    standingsId: number;
    scheduleId: number;

    @ViewChild('containerEl')
    containerEl: ElementRef;

    loaderHeight = 0;
    isLoaderVisible = true;

    get isShowLoader() {
        return !this._allReportsReady();
    }

    constructor(
        private _dataService: TournamentDataService) { }

    ngOnInit(): void {
        this._updateHeight();

        this._dataService
            .getTournamentMetadata()
            .toPromise()
            .then(data => {
                this._updateIds(data.mode, data.id);
                this._updateLoaderState();
            });
    }

    onReportReady(report: string) {
        this.reportsStates[report] = true;

        // setTimeout needed to allow browser render changes and return valid height for wrapper
        setTimeout(() => this._updateLoaderState(), 200);
    }

    private _updateHeight() {

        if (!this.containerEl) {
            this.loaderHeight = 0;
        }

        this.loaderHeight = this.containerEl.nativeElement.offsetHeight;
    }

    private _updateLoaderState() {
        this._updateHeight();
        this.isLoaderVisible = !this._allReportsReady();
    }

    private _allReportsReady() {
        return this.reportsStates.standings
            && this.reportsStates.pivot
            && this.reportsStates.schedule;
    }

    private _updateIds(modes: string[], id: number): void {
        modes.forEach(mode => {
            switch (mode) {
                case 'pivot':
                    this.pivotId = id;
                    this.reportsStates.pivot = false;
                    break;
                case 'standings':
                    this.standingsId = id;
                    this.reportsStates.standings = false;
                    break;
                case 'schedule':
                    this.scheduleId = id;
                    this.reportsStates.schedule = false;
                    break;
            }
        });
    }
}
