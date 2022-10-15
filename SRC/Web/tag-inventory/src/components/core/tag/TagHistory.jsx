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
import { getTagHistory } from "../../../actions/tagActions";
import Table from "../../common/ui-controls/Table";
import { connect } from "react-redux";
import { Link } from "react-router-dom";
import moment from "moment";
import ChildTimeLine from "../../common/ui-controls/ChildTimeLine";
import EventNote from "@material-ui/icons/EventNote";
import Typography from "@material-ui/core/Typography";

import PropTypes from "prop-types";

const TagHistory = (props) => {
  const tagID = props.match.params.tagID;
  let lastItem = false;
  let counter = 0;

  const columns = [
    { path: "updatedDate", title: "Date and Time", key: 1 },
    { path: "status", title: "Tag Status", key: 1 },
  ];

  const handleClick = (arg) => {};

  useEffect(() => {
    const url = `${process.env.REACT_APP_API_URL}tag/history/${tagID}`;

    props.getTagHistoryAction(url).then((r) => {});
  }, []);

  const formatDate = (dt) => moment(dt).format("dddd DD MMMM YYYY");

  return (
    <React.Fragment>
      <div className="actions">
        <Actions isFormValid={true} onLinkClick={handleClick}></Actions>
      </div>
      <div className="title-summary-wrapper">
        <div className="content-title">
          <h3>{tagID}</h3>
        </div>
        <div className="shipment-detail-summary">
          <Link to={`/tag/${tagID}`}>Summary</Link> &nbsp; | &nbsp;
          <span>History</span>
        </div>
      </div>
      <div className="content-area">
        <div className="row">
          <div className="col-md-5 time-line-container">
            <Timeline>
              {props.tagHistory.map((t) => {
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
                      tags={t.tags}
                      formatDate={formatDate}
                      lastItem={props.tagHistory.length === counter}
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

const mapStateToProp = (state) => {
  return {
    tagHistory: state.tag.tagHistory,
  };
};

const mapDispatchToProps = (dispatch) => ({
  getTagHistoryAction: (url) => dispatch(getTagHistory(url)),
});

export default connect(mapStateToProp, mapDispatchToProps)(TagHistory);
