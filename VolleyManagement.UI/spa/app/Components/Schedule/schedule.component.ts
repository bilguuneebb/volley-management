import { Component, OnInit, Input, OnDestroy, Output, EventEmitter } from '@angular/core';

import 'rxjs/add/operator/toPromise';

import { ScheduleModel } from '../../Models/Schedule/Schedule';
import { ScheduleService } from '../../Services/schedule.service';
import { GameResult } from '../../Models/Schedule/GameResult';
import { ScheduleDay } from '../../Models/Schedule/ScheduleDay';


@Component({
    selector: 'schedule',
    templateUrl: './schedule.component.html',
    styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {

    @Input() scheduleId: number;
    @Output() ready: EventEmitter<void> = new EventEmitter<void>();

    data: ScheduleModel = {} as ScheduleModel;
    divisionsIds: number[] = [];

    constructor(private scheduleService: ScheduleService) { }

    ngOnInit() {
        this.scheduleService
            .getSchedule(this.scheduleId)
            .toPromise()
            .then(data => {
                this.data = data;
                this.ready.emit();
                this._getSortedDivisionsIds();
            });
    }

    gameIsPlayed(gameResult: GameResult) {
        return gameResult.AwayTeamName &&
            gameResult.Result &&
            (!gameResult.Result.TotalScore.IsEmpty || gameResult.Result.IsTechnicalDefeat);
    }

    getdivisionsHeader(day: ScheduleDay): string {
        let info = '';
        day.Divisions.forEach((item) => {
            info += `${item.Name}: ${item.Rounds.join()} тур. `;
        });
        return info;
    }

    getDivisionAccentColor(divisionId: number): string {
        let index = this.divisionsIds.indexOf(divisionId);
        return 'division' + ++index;
    }

    isFreeDay(gameResult: GameResult): boolean {
        return !gameResult.AwayTeamName;
    }

    private _getSortedDivisionsIds() {
        this.data.Schedule.forEach((item) => {
            item.Days.forEach((it) => {
                it.Divisions.forEach(d => {
                    if (this.divisionsIds.indexOf(d.Id) === -1) {
                        this.divisionsIds.push(d.Id);
                    }
                });
            });
        });

        this.divisionsIds.sort((a, b) => a - b);
    }
}
