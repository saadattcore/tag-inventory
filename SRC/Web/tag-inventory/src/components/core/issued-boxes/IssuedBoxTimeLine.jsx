import React, { useEffect } from "react";
import Actions from "../../common/reuseable/Actions";
import {
  Timeline,
  TimelineItem,
  TimelineSeparator,
  TimelineDot,
  TimelineConnector,
  TimelineContent,
  TimelineOppositeContent,
} from "@material-ui/lab";
import { connect } from "react-redux";
import { Link } from "react-router-dom";
import moment from "moment";
import ChildTimeLine from "../../common/ui-controls/ChildTimeLine";
import Typography from "@material-ui/core/Typography";
import PropTypes from "prop-types";

const IssuedBoxTimeLine = (props) => {
  let counter = 0;

  const formatDate = (dt) => moment(dt).format("dddd DD MMMM YYYY");

  return (
    <React.Fragment>
      <div className="content-area-summary">
        <div className="row">
          <div className="col-md-5 time-line-container">
            <Timeline>
              {props.issuedBoxHistory.map((t) => {
                {
                  counter++;
                }
                return (
                  <React.Fragment>
                    <TimelineItem style={{ color: "#1976d2" }}>
                      <TimelineSeparator>
                        <TimelineDot>{/*    <EventNote /> */}</TimelineDot>
                        <TimelineConnector />
                      </TimelineSeparator>
                      <TimelineContent>
                        <Typography variant="h6" component="span">
                          {formatDate(t.dateGroup)}
                        </Typography>
                      </TimelineContent>
                    </TimelineItem>
                    <ChildTimeLine
                      isHeightSpecify={true}
                      tags={t.issuedBoxList}
                      formatDate={formatDate}
                      lastItem={props.issuedBoxHistory.length === counter}
                      dataHaveSubField={
                        t.issuedBoxList &&
                        Object.keys(t.issuedBoxList[0]).find(
                          (k) => k === "quantity"
                        )
                      }
                    ></ChildTimeLine>
                  </React.Fragment>
                );
              })}
            </Timeline>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

export default IssuedBoxTimeLine;
