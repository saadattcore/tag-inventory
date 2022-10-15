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

import PropTypes, { object } from "prop-types";

const ChildTimeLine = ({
  dateGroup,
  tags,
  formatDate,
  lastItem,
  dataHaveSubField,
  isHeightSpecify,
}) => {
  let counter = 0;
  return (
    <React.Fragment>
      {tags.map((t) => {
        counter++;
        return (
          <React.Fragment>
            <TimelineItem
              className={isHeightSpecify ? "timeline-item-height" : ""}
            >
              <TimelineSeparator>
                <TimelineDot variant="outlined" />
                {!lastItem ? (
                  <TimelineConnector />
                ) : (
                  counter < tags.length && <TimelineConnector />
                )}
              </TimelineSeparator>
              <TimelineContent>
                {dataHaveSubField ? (
                  <b>{moment(t.updatedDate).format("hh:mm")}</b>
                ) : (
                  moment(t.updatedDate).format("hh:mm")
                )}
                &nbsp;&nbsp;{dataHaveSubField ? <b>{t.status}</b> : t.status}
                <br />
                {dataHaveSubField && `Quantity: ${t.quantity}`}
              </TimelineContent>
            </TimelineItem>
          </React.Fragment>
        );
      })}
    </React.Fragment>
  );
};

export default ChildTimeLine;
