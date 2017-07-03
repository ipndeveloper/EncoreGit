Gotchas:

1. The entities in the edmx can't have recursive navigation properties.  You shouldn't need them anyway.  Come see me if you have a problem.
  This includes Orders and OrderItems.  I found I had to remove those from the edmx manually (i.e. edit using an xml editor).


